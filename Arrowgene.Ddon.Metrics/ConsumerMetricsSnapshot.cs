using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Metrics
{
    public readonly struct ConsumerMetricsSnapshot
    {
        // Bucket labels for display/export
        public static readonly string[] DurationBucketLabels =
        {
            "<100us", "<500us", "<1ms", "<5ms", "<10ms",
            "<50ms", "<100ms", "<500ms", "<1s", ">=1s"
        };

        public ConsumerMetricsSnapshot(
            long handlersExecuted,
            long handlerErrors,
            long[] handlerDurationBuckets,
            IReadOnlyDictionary<string, HandlerMetrics> handlerMetrics)
        {
            HandlersExecuted = handlersExecuted;
            HandlerErrors = handlerErrors;
            HandlerDurationBuckets = handlerDurationBuckets;
            Handlers = handlerMetrics ?? new Dictionary<string, HandlerMetrics>();
        }

        public long HandlersExecuted { get; }
        public long HandlerErrors { get; }
        public ReadOnlyMemory<long> HandlerDurationBuckets { get; }
        public IReadOnlyDictionary<string, HandlerMetrics> Handlers { get; }

        public readonly struct HandlerMetrics
        {
            public HandlerMetrics(
                string handlerName,
                long executionCount,
                long errorCount,
                long totalDurationTicks,
                long minDurationTicks,
                long maxDurationTicks)
            {
                HandlerName = handlerName ?? string.Empty;
                ExecutionCount = executionCount;
                ErrorCount = errorCount;
                TotalDurationTicks = totalDurationTicks;
                MinDurationTicks = minDurationTicks;
                MaxDurationTicks = maxDurationTicks;
            }

            public string HandlerName { get; }
            public long ExecutionCount { get; }
            public long ErrorCount { get; }
            public long TotalDurationTicks { get; }
            public long MinDurationTicks { get; }
            public long MaxDurationTicks { get; }

            public TimeSpan TotalDuration => new(TotalDurationTicks);
            public TimeSpan MinDuration => new(MinDurationTicks);
            public TimeSpan MaxDuration => new(MaxDurationTicks);
            public TimeSpan AvgDuration => ExecutionCount > 0
                ? new TimeSpan(TotalDurationTicks / ExecutionCount)
                : TimeSpan.Zero;
        }
    }
}
