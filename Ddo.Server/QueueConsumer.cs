using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Arrowgene.Services;
using Arrowgene.Services.Logging;
using Arrowgene.Services.Networking.Tcp;
using Arrowgene.Services.Networking.Tcp.Consumer;
using Arrowgene.Services.Networking.Tcp.Consumer.BlockingQueueConsumption;
using Arrowgene.Services.Networking.Tcp.Server.AsyncEvent;
using Ddo.Server.Logging;
using Ddo.Server.Model;
using Ddo.Server.Packet;
using Ddo.Server.Setting;

namespace Ddo.Server
{
    public class QueueConsumer : IConsumer
    {
        public const int NoExpectedSize = -1;

        private readonly BlockingCollection<ClientEvent>[] _queues;
        private readonly Thread[] _threads;
        private readonly Dictionary<int, IClientHandler> _clientHandlers;
        private readonly Dictionary<int, IConnectionHandler> _connectionHandlers;
        private readonly Dictionary<ITcpSocket, DdoConnection> _connections;
        private readonly DdoLogger _logger;
        private readonly object _lock;
        private readonly int _maxUnitOfOrder;
        private DdoSetting _setting;
        private volatile bool _isRunning;

        private CancellationTokenSource _cancellationTokenSource;

        public int HandlersCount => _clientHandlers.Count;

        public Action<DdoConnection> ClientDisconnected;
        public Action<DdoConnection> ClientConnected;
        public Action Started;
        public Action Stopped;

        public QueueConsumer(DdoSetting setting, AsyncEventSettings socketSetting)
        {
            _setting = setting;
            _logger = LogProvider.Logger<DdoLogger>(this);
            _maxUnitOfOrder = socketSetting.MaxUnitOfOrder;
            _queues = new BlockingCollection<ClientEvent>[_maxUnitOfOrder];
            _threads = new Thread[_maxUnitOfOrder];
            _lock = new object();
            _clientHandlers = new Dictionary<int, IClientHandler>();
            _connectionHandlers = new Dictionary<int, IConnectionHandler>();
            _connections = new Dictionary<ITcpSocket, DdoConnection>();
        }

        public void Clear()
        {
            _clientHandlers.Clear();
            _connectionHandlers.Clear();
        }

        public void AddHandler(IClientHandler clientHandler, bool overwrite = false)
        {
            if (overwrite)
            {
                if (_clientHandlers.ContainsKey(clientHandler.Id))
                {
                    _clientHandlers[clientHandler.Id] = clientHandler;
                }
                else
                {
                    _clientHandlers.Add(clientHandler.Id, clientHandler);
                }

                return;
            }

            if (_clientHandlers.ContainsKey(clientHandler.Id))
            {
                _logger.Error($"ClientHandlerId: {clientHandler.Id} already exists");
            }
            else
            {
                _clientHandlers.Add(clientHandler.Id, clientHandler);
            }
        }

        public void AddHandler(IConnectionHandler connectionHandler, bool overwrite = false)
        {
            if (overwrite)
            {
                if (_connectionHandlers.ContainsKey(connectionHandler.Id))
                {
                    _connectionHandlers[connectionHandler.Id] = connectionHandler;
                }
                else
                {
                    _connectionHandlers.Add(connectionHandler.Id, connectionHandler);
                }

                return;
            }

            if (_connectionHandlers.ContainsKey(connectionHandler.Id))
            {
                _logger.Error($"ConnectionHandlerId: {connectionHandler.Id} already exists");
            }
            else
            {
                _connectionHandlers.Add(connectionHandler.Id, connectionHandler);
            }
        }

        private void HandleReceived(ITcpSocket socket, byte[] data)
        {
            if (!socket.IsAlive)
            {
                return;
            }

            DdoConnection connection;
            lock (_lock)
            {
                if (!_connections.ContainsKey(socket))
                {
                    _logger.Error(socket, $"Client does not exist in lookup");
                    return;
                }

                connection = _connections[socket];
            }

            List<DdoPacket> packets = connection.Receive(data);
            foreach (DdoPacket packet in packets)
            {
                DdoClient client = connection.Client;
                if (client != null)
                {
                    HandleReceived_Client(client, packet);
                }
                else
                {
                    HandleReceived_Connection(connection, packet);
                }
            }
        }

        private void HandleReceived_Connection(DdoConnection connection, DdoPacket packet)
        {
            if (!_connectionHandlers.ContainsKey(packet.Id))
            {
             //   _logger.LogUnknownIncomingPacket(connection, packet);
                return;
            }

            IConnectionHandler connectionHandler = _connectionHandlers[packet.Id];
            if (connectionHandler.ExpectedSize != NoExpectedSize && packet.Data.Size < connectionHandler.ExpectedSize)
            {
                _logger.Error(connection,
                    $"Ignoring Packed (Id:{packet.Id}) is smaller ({packet.Data.Size}) than expected ({connectionHandler.ExpectedSize})");
                return;
            }

          //  _logger.LogIncomingPacket(connection, packet);
            packet.Data.SetPositionStart();
            try
            {
                connectionHandler.Handle(connection, packet);
            }
            catch (Exception ex)
            {
                _logger.Exception(connection, ex);
            }
        }

        private void HandleReceived_Client(DdoClient client, DdoPacket packet)
        {
            if (!_clientHandlers.ContainsKey(packet.Id))
            {
                //_logger.LogUnknownIncomingPacket(client, packet);
                return;
            }

            IClientHandler clientHandler = _clientHandlers[packet.Id];
            if (clientHandler.ExpectedSize != NoExpectedSize && packet.Data.Size < clientHandler.ExpectedSize)
            {
                _logger.Error(client,
                    $"Ignoring Packed (Id:{packet.Id}) is smaller ({packet.Data.Size}) than expected ({clientHandler.ExpectedSize})");
                return;
            }

          //  _logger.LogIncomingPacket(client, packet);
            packet.Data.SetPositionStart();
            try
            {
                clientHandler.Handle(client, packet);
            }
            catch (Exception ex)
            {
                _logger.Exception(client, ex);
            }
        }

        private void HandleDisconnected(ITcpSocket socket)
        {
            DdoConnection connection;
            lock (_lock)
            {
                if (!_connections.ContainsKey(socket))
                {
                    _logger.Error(socket, $"Disconnected client does not exist in lookup");
                    return;
                }

                connection = _connections[socket];
                _connections.Remove(socket);
                _logger.Debug($"Clients Count: {_connections.Count}");
            }

            Action<DdoConnection> onClientDisconnected = ClientDisconnected;
            if (onClientDisconnected != null)
            {
                try
                {
                    onClientDisconnected.Invoke(connection);
                }
                catch (Exception ex)
                {
                    _logger.Exception(connection, ex);
                }
            }

            _logger.Info(connection, $"Client disconnected");
        }

        private void HandleConnected(ITcpSocket socket)
        {
            DdoConnection connection = new DdoConnection(socket, new PacketFactory(_setting));
            lock (_lock)
            {
                _connections.Add(socket, connection);
                _logger.Debug($"Clients Count: {_connections.Count}");
            }

            Action<DdoConnection> onClientConnected = ClientConnected;
            if (onClientConnected != null)
            {
                try
                {
                    onClientConnected.Invoke(connection);
                }
                catch (Exception ex)
                {
                    _logger.Exception(connection, ex);
                }
            }

            _logger.Info(connection, $"Client connected");
        }

        private void Consume(int unitOfOrder)
        {
            while (_isRunning)
            {
                ClientEvent clientEvent;
                try
                {
                    clientEvent = _queues[unitOfOrder].Take(_cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    return;
                }

                switch (clientEvent.ClientEventType)
                {
                    case ClientEventType.ReceivedData:
                        HandleReceived(clientEvent.Socket, clientEvent.Data);
                        break;
                    case ClientEventType.Connected:
                        HandleConnected(clientEvent.Socket);
                        break;
                    case ClientEventType.Disconnected:
                        HandleDisconnected(clientEvent.Socket);
                        break;
                }
            }
        }

        void IConsumer.OnStart()
        {
            if (_isRunning)
            {
                _logger.Error($"Consumer already running.");
                return;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            _isRunning = true;
            for (int i = 0; i < _maxUnitOfOrder; i++)
            {
                int uuo = i;
                _queues[i] = new BlockingCollection<ClientEvent>();
                _threads[i] = new Thread(() => Consume(uuo));
                _threads[i].Name = $"Consumer: {i}";
                _logger.Info($"Starting Consumer: {i}");
                _threads[i].Start();
            }
        }

        public void OnStarted()
        {
            Action started = Started;
            if (started != null)
            {
                started.Invoke();
            }
        }

        void IConsumer.OnReceivedData(ITcpSocket socket, byte[] data)
        {
            _queues[socket.UnitOfOrder].Add(new ClientEvent(socket, ClientEventType.ReceivedData, data));
        }

        void IConsumer.OnClientDisconnected(ITcpSocket socket)
        {
            _queues[socket.UnitOfOrder].Add(new ClientEvent(socket, ClientEventType.Disconnected));
        }

        void IConsumer.OnClientConnected(ITcpSocket socket)
        {
            _queues[socket.UnitOfOrder].Add(new ClientEvent(socket, ClientEventType.Connected));
        }

        void IConsumer.OnStop()
        {
            if (!_isRunning)
            {
                _logger.Error($"Consumer already stopped.");
                return;
            }

            _isRunning = false;
            _cancellationTokenSource.Cancel();
            for (int i = 0; i < _maxUnitOfOrder; i++)
            {
                Thread consumerThread = _threads[i];
                _logger.Info($"Shutting Consumer: {i} down...");
                Service.JoinThread(consumerThread, 10000, _logger);
                _logger.Info($"Consumer: {i} ended.");
                _threads[i] = null;
            }
        }

        public void OnStopped()
        {
            Action stopped = Stopped;
            if (stopped != null)
            {
                stopped.Invoke();
            }
        }
    }
}
