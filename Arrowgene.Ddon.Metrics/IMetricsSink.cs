namespace Arrowgene.Ddon.Metrics
{
    public interface IMetricsSink : System.IDisposable
    {
        void WriteTcpServerMetrics(MetricsSample sample);
    }
}
