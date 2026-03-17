using System.Threading;

namespace Arrowgene.Ddon.Metrics
{
    internal sealed class DdonServerMetricsState
    {
        private readonly ConsumerMetricsState _consumerMetricsState;
        private int _captureEnabled;

        public DdonServerMetricsState(ConsumerMetricsState consumerMetricsState)
        {
            _consumerMetricsState = consumerMetricsState;
        }

        public void EnableCapture()
        {
            Interlocked.Exchange(ref _captureEnabled, 1);
            _consumerMetricsState.EnableCapture();
        }

        public void DisableCapture()
        {
            Interlocked.Exchange(ref _captureEnabled, 0);
            _consumerMetricsState.DisableCapture();
        }

        public DdonServerMetricsSnapshot CreateSnapshot()
        {
            ConsumerMetricsSnapshot consumerSnapshot = _consumerMetricsState.CreateSnapshot();
            return new DdonServerMetricsSnapshot(consumerSnapshot);
        }
    }
}
