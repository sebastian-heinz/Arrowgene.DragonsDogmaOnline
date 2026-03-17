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
            IReadOnlyList<DdonHandlerDurationSnapshot> handlerDurations)
        {
            HandlersExecuted = handlersExecuted;
            HandlerErrors = handlerErrors;
            HandlerDurationBuckets = handlerDurationBuckets;
            HandlerDurations = handlerDurations ?? Array.Empty<DdonHandlerDurationSnapshot>();
        }

        public long HandlersExecuted { get; }
        public long HandlerErrors { get; }
        public ReadOnlyMemory<long> HandlerDurationBuckets { get; }
        public IReadOnlyList<DdonHandlerDurationSnapshot> HandlerDurations { get; }
    }
}
