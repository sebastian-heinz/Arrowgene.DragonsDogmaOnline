using System;

namespace Arrowgene.Ddon.Metrics
{
    public readonly struct DdonServerMetricsSnapshot
    {
        public DdonServerMetricsSnapshot(
            DateTime timestampUtc,
            DateTime serverStartedAtUtc,
            long sequenceNumber,
            double handlersExecutedPerSecond,
            double handlerErrorsPerSecond,
            ConsumerMetricsSnapshot consumerMetrics)
        {
            TimestampUtc = timestampUtc;
            ServerStartedAtUtc = serverStartedAtUtc;
            SequenceNumber = sequenceNumber;
            HandlersExecutedPerSecond = handlersExecutedPerSecond;
            HandlerErrorsPerSecond = handlerErrorsPerSecond;
            ConsumerMetrics = consumerMetrics;
        }

        public DateTime TimestampUtc { get; }
        public DateTime ServerStartedAtUtc { get; }
        public long SequenceNumber { get; }
        public double HandlersExecutedPerSecond { get; }
        public double HandlerErrorsPerSecond { get; }
        public ConsumerMetricsSnapshot ConsumerMetrics { get; }

        public TimeSpan Uptime => ServerStartedAtUtc == DateTime.MinValue
            ? TimeSpan.Zero
            : TimestampUtc - ServerStartedAtUtc;
    }
}
