using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Metrics
{
    public readonly struct DdonConsumerMetricsSnapshot
    {
        // Bucket labels for display/export (matches networking library scheme)
        public static readonly string[] DurationBucketLabels =
        {
            "<100us", "100us-1ms", "1-10ms", "10-50ms", "50-250ms",
            "250ms-1s", "1-5s", "5-30s", "30s-2m", ">=2m"
        };

        public DdonConsumerMetricsSnapshot(
            long handlersExecuted,
            long handlerErrors,
            long[] handlerDurationBuckets,
            long[] parseDurationBuckets,
            IReadOnlyDictionary<string, HandlerMetrics> handlerMetrics)
        {
            HandlersExecuted = handlersExecuted;
            HandlerErrors = handlerErrors;
            HandlerDurationBuckets = handlerDurationBuckets;
            ParseDurationBuckets = parseDurationBuckets;
            Handlers = handlerMetrics ?? new Dictionary<string, HandlerMetrics>();
        }

        public long HandlersExecuted { get; }
        public long HandlerErrors { get; }
        public ReadOnlyMemory<long> HandlerDurationBuckets { get; }
        public ReadOnlyMemory<long> ParseDurationBuckets { get; }
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
