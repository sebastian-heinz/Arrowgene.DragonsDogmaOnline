using System.Threading;

namespace Arrowgene.Ddon.Metrics
{
    internal sealed class DdonServerMetricsState
    {
        internal readonly ConsumerMetricsState ConsumerMetricsState;
        private int _captureEnabled;

        public DdonServerMetricsState(ConsumerMetricsState consumerMetricsState)
        {
            ConsumerMetricsState = consumerMetricsState;
        }

        internal void EnableCapture()
        {
            Volatile.Write(ref _captureEnabled, 1);
        }

        internal void DisableCapture()
        {
            Volatile.Write(ref _captureEnabled, 0);
        }

        internal bool IsCaptureEnabled()
        {
            return Volatile.Read(ref _captureEnabled) == 1;
        }
    }
}
