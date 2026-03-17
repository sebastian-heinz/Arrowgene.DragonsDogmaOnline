using System.Collections.Generic;

namespace Arrowgene.Ddon.Metrics
{
    public readonly struct DdonServerMetricsSnapshot
    {
        public DdonServerMetricsSnapshot(ConsumerMetricsSnapshot consumerMetrics)
        {
            ConsumerMetrics = consumerMetrics;
        }

        public ConsumerMetricsSnapshot ConsumerMetrics { get; }

        // Convenience accessor (delegates to consumer)
        public IReadOnlyList<DdonHandlerDurationSnapshot> HandlerDurations => ConsumerMetrics.HandlerDurations;
    }
}
