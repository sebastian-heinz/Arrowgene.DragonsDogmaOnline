using System;
using Arrowgene.Networking.SAEAServer.Metric;

namespace Arrowgene.Ddon.Metrics
{
    public class MetricsSample
    {
        public MetricsSample(
            DateTime capturedAtUtc,
            int serverId,
            string serverName,
            string serverType,
            string serverIdentity,
            TcpServerMetricsSnapshot tcpServerMetricsSnapshot,
            DdonServerMetricsSnapshot? ddonServerMetricsSnapshot = null
        )
        {
            CapturedAtUtc = capturedAtUtc;
            ServerId = serverId;
            ServerName = serverName ?? string.Empty;
            ServerType = serverType ?? string.Empty;
            ServerIdentity = serverIdentity ?? string.Empty;
            TcpServerMetricsSnapshot = tcpServerMetricsSnapshot;
            DdonServerMetricsSnapshot = ddonServerMetricsSnapshot;
        }

        public DateTime CapturedAtUtc { get; }
        public int ServerId { get; }
        public string ServerName { get; }
        public string ServerType { get; }
        public string ServerIdentity { get; }
        public TcpServerMetricsSnapshot TcpServerMetricsSnapshot { get; }
        public DdonServerMetricsSnapshot? DdonServerMetricsSnapshot { get; }
    }
}
