using System.Threading;

namespace Arrowgene.Ddon.Metrics
{
    internal sealed class DdonServerMetricsState
    {
        private int _captureEnabled;
        private long _sequenceNumber;
        private long _previousHandlersExecuted;
        private long _previousHandlerErrors;

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

        internal long IncrementSequenceNumber()
        {
            return Interlocked.Increment(ref _sequenceNumber);
        }

        internal (double executedPerSec, double errorsPerSec) CalculateRates(
            long currentExecuted, long currentErrors, double elapsedSeconds)
        {
            long deltaExecuted = currentExecuted - Volatile.Read(ref _previousHandlersExecuted);
            long deltaErrors = currentErrors - Volatile.Read(ref _previousHandlerErrors);

            Volatile.Write(ref _previousHandlersExecuted, currentExecuted);
            Volatile.Write(ref _previousHandlerErrors, currentErrors);

            double executedPerSec = elapsedSeconds > 0 ? deltaExecuted / elapsedSeconds : 0;
            double errorsPerSec = elapsedSeconds > 0 ? deltaErrors / elapsedSeconds : 0;

            return (executedPerSec, errorsPerSec);
        }
    }
}
