namespace Arrowgene.Ddon.Metrics
{
    public sealed class NullMetricsSink : IMetricsSink
    {
        public static readonly NullMetricsSink Instance = new();

        private NullMetricsSink()
        {
        }

        public void WriteTcpServerMetrics(MetricsSample sample)
        {
        }

        public void Dispose()
        {
        }
    }
}
