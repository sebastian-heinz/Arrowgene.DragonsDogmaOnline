using System;
using Arrowgene.Networking.SAEAServer.Metric;

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
            DdonConsumerMetricsSnapshot ddonConsumerMetrics,
            TcpServerMetricsSnapshot tcpServerMetrics
        )
        {
            TimestampUtc = timestampUtc;
            ServerStartedAtUtc = serverStartedAtUtc;
            SequenceNumber = sequenceNumber;
            HandlersExecutedPerSecond = handlersExecutedPerSecond;
            HandlerErrorsPerSecond = handlerErrorsPerSecond;
            DdonConsumerMetrics = ddonConsumerMetrics;
            TcpServerMetrics = tcpServerMetrics;
        }

        public DateTime TimestampUtc { get; }
        public DateTime ServerStartedAtUtc { get; }
        public long SequenceNumber { get; }
        public double HandlersExecutedPerSecond { get; }
        public double HandlerErrorsPerSecond { get; }
        public DdonConsumerMetricsSnapshot DdonConsumerMetrics { get; }
        public TcpServerMetricsSnapshot TcpServerMetrics { get; }

        public TimeSpan Uptime => ServerStartedAtUtc == DateTime.MinValue
            ? TimeSpan.Zero
            : TimestampUtc - ServerStartedAtUtc;
    }
}
