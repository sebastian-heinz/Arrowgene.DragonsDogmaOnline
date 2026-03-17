using System;
using System.Threading;
using Arrowgene.Networking.SAEAServer.Metric;

namespace Arrowgene.Ddon.Metrics
{
    public class DdonServerMetricsCapture : IDisposable
    {
        private readonly Func<TcpServerMetricsSnapshot> _captureTcpServerMetricsSnapshot;
        private readonly Func<DdonServerMetricsSnapshot> _captureDdonServerMetricsSnapshot;
        private readonly Action<Exception> _errorHandler;
        private readonly string _serverIdentity;
        private readonly string _serverName;
        private readonly string _serverType;
        private readonly int _serverId;
        private readonly ManualResetEventSlim _stopSignal;
        private readonly TimeSpan _captureInterval;

        private IMetricsSink _metricsSink;
        private Thread _thread;

        public DdonServerMetricsCapture(
            int serverId,
            string serverName,
            string serverType,
            string serverIdentity,
            Func<TcpServerMetricsSnapshot> captureTcpServerMetricsSnapshot,
            TimeSpan captureInterval,
            Action<Exception> errorHandler = null,
            Func<DdonServerMetricsSnapshot> captureDdonServerMetricsSnapshot = null
        )
        {
            _serverId = serverId;
            _serverName = serverName ?? string.Empty;
            _serverType = serverType ?? string.Empty;
            _serverIdentity = serverIdentity ?? string.Empty;
            _captureTcpServerMetricsSnapshot = captureTcpServerMetricsSnapshot ?? throw new ArgumentNullException(nameof(captureTcpServerMetricsSnapshot));
            _captureDdonServerMetricsSnapshot = captureDdonServerMetricsSnapshot;
            _captureInterval = captureInterval;
            _errorHandler = errorHandler;
            _metricsSink = NullMetricsSink.Instance;
            _stopSignal = new ManualResetEventSlim(false);
        }

        public IMetricsSink MetricsSink
        {
            get => _metricsSink;
            set => _metricsSink = value ?? NullMetricsSink.Instance;
        }

        public void Start()
        {
            if (_thread != null && _thread.IsAlive)
            {
                return;
            }

            _stopSignal.Reset();
            _thread = new Thread(CaptureLoop)
            {
                IsBackground = true,
                Name = $"{_serverIdentity}.Metrics"
            };
            _thread.Start();
        }

        public void Stop()
        {
            _stopSignal.Set();
            _thread?.Join();
            _thread = null;
        }

        public void Dispose()
        {
            Stop();
            IMetricsSink metricsSink = MetricsSink;
            MetricsSink = NullMetricsSink.Instance;
            metricsSink.Dispose();
        }

        private void CaptureLoop()
        {
            while (!_stopSignal.IsSet)
            {
                try
                {
                    MetricsSink.WriteTcpServerMetrics(new MetricsSample(
                        DateTime.UtcNow,
                        _serverId,
                        _serverName,
                        _serverType,
                        _serverIdentity,
                        _captureTcpServerMetricsSnapshot(),
                        _captureDdonServerMetricsSnapshot()
                    ));
                }
                catch (Exception ex)
                {
                    _errorHandler?.Invoke(ex);
                }

                _stopSignal.Wait(_captureInterval);
            }
        }
    }
}
