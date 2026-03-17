using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Arrowgene.Ddon.Metrics
{
    internal sealed class ConsumerMetricsState
    {
        // Handler duration histogram buckets (in microseconds)
        // Bucket boundaries: 100us, 500us, 1ms, 5ms, 10ms, 50ms, 100ms, 500ms, 1s, >1s
        private static readonly long[] DurationBucketBoundariesUs =
        {
            100, 500, 1_000, 5_000, 10_000, 50_000, 100_000, 500_000, 1_000_000
        };

        private const int DurationBucketCount = 10; // 9 boundaries + 1 overflow

        private readonly long[] _handlerDurationBuckets = new long[DurationBucketCount];
        private long _handlersExecuted;
        private long _handlerErrors;
        private int _captureEnabled; // 0 = disabled, 1 = enabled

        // Per-handler detail tracking (lock-free queue, drained on snapshot)
        private readonly ConcurrentQueue<DdonHandlerDurationSnapshot> _handlerDurations = new();

        public void EnableCapture() => Interlocked.Exchange(ref _captureEnabled, 1);
        public void DisableCapture() => Interlocked.Exchange(ref _captureEnabled, 0);
        private bool IsCaptureEnabled() => Volatile.Read(ref _captureEnabled) == 1;

        public void RecordHandlerExecution(
            string handlerId,
            string handlerName,
            string clientIdentity,
            long startTimestamp)
        {
            if (!IsCaptureEnabled()) return;

            TimeSpan elapsed = Stopwatch.GetElapsedTime(startTimestamp);
            long elapsedUs = (long)(elapsed.TotalMicroseconds);

            Interlocked.Increment(ref _handlersExecuted);

            // Bucket the duration
            int bucket = DurationBucketCount - 1; // default to overflow
            for (int i = 0; i < DurationBucketBoundariesUs.Length; i++)
            {
                if (elapsedUs < DurationBucketBoundariesUs[i])
                {
                    bucket = i;
                    break;
                }
            }
            Interlocked.Increment(ref _handlerDurationBuckets[bucket]);

            // Enqueue detailed per-handler snapshot
            _handlerDurations.Enqueue(new DdonHandlerDurationSnapshot(
                handlerId, handlerName, clientIdentity, elapsed));
        }

        public void IncrementHandlerErrors()
        {
            if (!IsCaptureEnabled()) return;
            Interlocked.Increment(ref _handlerErrors);
        }

        public ConsumerMetricsSnapshot CreateSnapshot()
        {
            long[] durationBuckets = new long[DurationBucketCount];
            for (int i = 0; i < DurationBucketCount; i++)
            {
                durationBuckets[i] = Volatile.Read(ref _handlerDurationBuckets[i]);
            }

            List<DdonHandlerDurationSnapshot> handlerDurations = new();
            while (_handlerDurations.TryDequeue(out DdonHandlerDurationSnapshot hd))
            {
                handlerDurations.Add(hd);
            }

            return new ConsumerMetricsSnapshot(
                handlersExecuted: Volatile.Read(ref _handlersExecuted),
                handlerErrors: Volatile.Read(ref _handlerErrors),
                handlerDurationBuckets: durationBuckets,
                handlerDurations: handlerDurations
            );
        }
    }
}
