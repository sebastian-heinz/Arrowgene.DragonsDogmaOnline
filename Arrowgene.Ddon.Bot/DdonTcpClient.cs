using System.Net.Sockets;
using System.Reflection;

public sealed class DdonTcpClient : IDisposable
{
    private readonly SemaphoreSlim _lifecycleMutex = new SemaphoreSlim(1, 1);
    private readonly int _receiveBufferSize;

    private TcpClient _client;
    private NetworkStream _stream;
    private CancellationTokenSource _receiveLoopCts;
    private Task _receiveLoopTask;
    private int _disconnectRaised;
    private bool _isRunning;
    private bool _disposed;

    public DdonTcpClient(int receiveBufferSize = 8192)
    {
        if (receiveBufferSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(receiveBufferSize),
                "Receive buffer size must be greater than zero.");
        }

        _receiveBufferSize = receiveBufferSize;
        NoDelay = true;
        EnableKeepAlive = true;
    }

    public bool NoDelay { get; set; }

    public bool EnableKeepAlive { get; set; }

    public bool IsRunning
    {
        get { return _isRunning; }
    }

    public bool IsConnected
    {
        get
        {
            return _isRunning &&
                   _client != null &&
                   _client.Client != null &&
                   _client.Client.Connected;
        }
    }

    public event EventHandler Connected;

    public event EventHandler<DataReceivedEventArgs> DataReceived;

    public event EventHandler Disconnected;

    public event EventHandler<TcpClientErrorEventArgs> Error;

    public Task Start(string host, int port, CancellationToken cancellationToken = default(CancellationToken))
    {
        return StartAsync(host, port, cancellationToken);
    }

    public async Task StartAsync(string host, int port,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        ThrowIfDisposed();

        if (string.IsNullOrWhiteSpace(host))
        {
            throw new ArgumentException("Host is required.", nameof(host));
        }

        if (port < 1 || port > 65535)
        {
            throw new ArgumentOutOfRangeException(nameof(port), "Port must be between 1 and 65535.");
        }

        TcpClient client = null;
        NetworkStream stream = null;
        CancellationTokenSource receiveLoopCts = null;

        await _lifecycleMutex.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (_isRunning)
            {
                throw new InvalidOperationException("TCP client is already running.");
            }

            client = new TcpClient();
            client.NoDelay = NoDelay;
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, EnableKeepAlive);

            try
            {
                await client.ConnectAsync(host, port).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                throw;
            }

            stream = client.GetStream();
            receiveLoopCts = new CancellationTokenSource();

            _client = client;
            _stream = stream;
            _receiveLoopCts = receiveLoopCts;
            _disconnectRaised = 0;
            _isRunning = true;
            _receiveLoopTask = ReceiveLoopAsync(stream, receiveLoopCts.Token);

            client = null;
            stream = null;
            receiveLoopCts = null;
        }
        finally
        {
            _lifecycleMutex.Release();
        }

        try
        {
            RaiseConnected();
        }
        catch
        {
            await StopAsync(CancellationToken.None).ConfigureAwait(false);
            throw;
        }
        finally
        {
            DisposeResources(stream, client, receiveLoopCts);
        }
    }

    public Task Stop(CancellationToken cancellationToken = default(CancellationToken))
    {
        return StopAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        await StopAsyncCore(cancellationToken, true).ConfigureAwait(false);
    }

    private async Task StopAsyncCore(CancellationToken cancellationToken, bool throwIfDisposed)
    {
        if (throwIfDisposed)
        {
            ThrowIfDisposed();
        }

        Task receiveLoopTask = null;
        NetworkStream stream = null;
        TcpClient client = null;
        CancellationTokenSource receiveLoopCts = null;
        bool hadActiveConnection = false;

        await _lifecycleMutex.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            hadActiveConnection = _isRunning ||
                                  _receiveLoopTask != null ||
                                  _stream != null ||
                                  _client != null ||
                                  _receiveLoopCts != null;

            receiveLoopTask = _receiveLoopTask;
            stream = _stream;
            client = _client;
            receiveLoopCts = _receiveLoopCts;

            _receiveLoopTask = null;
            _stream = null;
            _client = null;
            _receiveLoopCts = null;
            _isRunning = false;
        }
        finally
        {
            _lifecycleMutex.Release();
        }

        if (!hadActiveConnection)
        {
            return;
        }

        if (receiveLoopCts != null)
        {
            try
            {
                receiveLoopCts.Cancel();
            }
            catch (ObjectDisposedException)
            {
            }
        }

        DisposeResources(stream, client, receiveLoopCts);

        if (receiveLoopTask != null)
        {
            try
            {
                await receiveLoopTask.ConfigureAwait(false);
            }
            catch
            {
            }
        }

        RaiseDisconnectedOnce();
    }

    public async Task SendAsync(byte[] data, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        await SendAsync(data, 0, data.Length, cancellationToken).ConfigureAwait(false);
    }

    public async Task SendAsync(byte[] data, int offset, int count,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        ThrowIfDisposed();

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        if (offset < 0 || offset > data.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset));
        }

        if (count < 0 || offset + count > data.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }

        NetworkStream stream;

        await _lifecycleMutex.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (!_isRunning || _stream == null)
            {
                throw new InvalidOperationException("TCP client is not connected.");
            }

            stream = _stream;
        }
        finally
        {
            _lifecycleMutex.Release();
        }

        try
        {
            await stream.WriteAsync(data, offset, count, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            RaiseError(ex);
            throw;
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        try
        {
            StopAsyncCore(CancellationToken.None, false).GetAwaiter().GetResult();
        }
        catch
        {
        }

        _disposed = true;
        _lifecycleMutex.Dispose();
    }

    private async Task ReceiveLoopAsync(NetworkStream stream, CancellationToken cancellationToken)
    {
        var buffer = new byte[_receiveBufferSize];

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)
                    .ConfigureAwait(false);
                if (bytesRead == 0)
                {
                    break;
                }

                var payload = new byte[bytesRead];
                Buffer.BlockCopy(buffer, 0, payload, 0, bytesRead);
                RaiseDataReceived(payload);
            }
        }
        catch (OperationCanceledException)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                throw;
            }
        }
        catch (ObjectDisposedException)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                throw;
            }
        }
        catch (IOException ex)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                RaiseError(ex);
            }
        }
        catch (Exception ex)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                RaiseError(ex);
            }
        }
        finally
        {
            await CleanupAfterReceiveLoopExitAsync().ConfigureAwait(false);
        }
    }

    private async Task CleanupAfterReceiveLoopExitAsync()
    {
        NetworkStream stream = null;
        TcpClient client = null;
        CancellationTokenSource receiveLoopCts = null;

        await _lifecycleMutex.WaitAsync().ConfigureAwait(false);
        try
        {
            stream = _stream;
            client = _client;
            receiveLoopCts = _receiveLoopCts;

            _receiveLoopTask = null;
            _stream = null;
            _client = null;
            _receiveLoopCts = null;
            _isRunning = false;
        }
        finally
        {
            _lifecycleMutex.Release();
        }

        DisposeResources(stream, client, receiveLoopCts);
        RaiseDisconnectedOnce();
    }

    private void RaiseConnected()
    {
        var handlers = Connected;
        if (handlers == null)
        {
            return;
        }

        foreach (EventHandler subscriber in handlers.GetInvocationList())
        {
            try
            {
                subscriber(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                RaiseError(UnwrapInvocationException(ex));
            }
        }
    }

    private void RaiseDataReceived(byte[] data)
    {
        var handlers = DataReceived;
        if (handlers == null)
        {
            return;
        }

        var args = new DataReceivedEventArgs(data);
        foreach (EventHandler<DataReceivedEventArgs> subscriber in handlers.GetInvocationList())
        {
            try
            {
                subscriber(this, args);
            }
            catch (Exception ex)
            {
                RaiseError(UnwrapInvocationException(ex));
            }
        }
    }

    private void RaiseDisconnectedOnce()
    {
        if (Interlocked.Exchange(ref _disconnectRaised, 1) != 0)
        {
            return;
        }

        var handlers = Disconnected;
        if (handlers == null)
        {
            return;
        }

        foreach (EventHandler subscriber in handlers.GetInvocationList())
        {
            try
            {
                subscriber(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                RaiseError(UnwrapInvocationException(ex));
            }
        }
    }

    private void RaiseError(Exception exception)
    {
        var handlers = Error;
        if (handlers == null)
        {
            return;
        }

        var args = new TcpClientErrorEventArgs(exception);
        foreach (EventHandler<TcpClientErrorEventArgs> subscriber in handlers.GetInvocationList())
        {
            try
            {
                subscriber(this, args);
            }
            catch
            {
            }
        }
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(typeof(TcpClient).FullName);
        }
    }

    private static Exception UnwrapInvocationException(Exception exception)
    {
        var invocationException = exception as TargetInvocationException;
        return invocationException != null && invocationException.InnerException != null
            ? invocationException.InnerException
            : exception;
    }

    private static void DisposeResources(NetworkStream stream, TcpClient client,
        CancellationTokenSource receiveLoopCts)
    {
        if (stream != null)
        {
            try
            {
                stream.Dispose();
            }
            catch
            {
            }
        }

        if (client != null)
        {
            try
            {
                client.Close();
            }
            catch
            {
            }
        }

        if (receiveLoopCts != null)
        {
            try
            {
                receiveLoopCts.Dispose();
            }
            catch
            {
            }
        }
    }
}

public sealed class DataReceivedEventArgs : EventArgs
{
    public DataReceivedEventArgs(byte[] data)
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        Data = data;
    }

    public byte[] Data { get; private set; }
}

public sealed class TcpClientErrorEventArgs : EventArgs
{
    public TcpClientErrorEventArgs(Exception exception)
    {
        if (exception == null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        Exception = exception;
    }

    public Exception Exception { get; private set; }
}
