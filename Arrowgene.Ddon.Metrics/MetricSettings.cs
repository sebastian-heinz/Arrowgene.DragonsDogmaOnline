using System.Runtime.Serialization;

namespace Arrowgene.Ddon.Metrics
{
    [DataContract]
    public class MetricSettings
    {
        [DataMember(Order = 1)] public bool Enabled { get; set; }
        [DataMember(Order = 10)] public MetricsSinkType MetricsSink { get; set; }
        [DataMember(Order = 20)] public int SamplingIntervalMs { get; set; }
        [DataMember(Order = 50)] public long FileMetricsSinkRetention { get; set; }
        [DataMember(Order = 51)] public string FileMetricsExportPath { get; set; }
        [DataMember(Order = 52)] public int FileMetricsExportIntervalMs { get; set; }


        public MetricSettings()
        {
            Enabled = false;
            MetricsSink = MetricsSinkType.FileMetricsSink;
            FileMetricsSinkRetention = 1000;
            FileMetricsExportPath = "metrics/";
            FileMetricsExportIntervalMs = 30000;
        }

        public MetricSettings(MetricSettings setting)
        {
            Enabled = setting.Enabled;
            MetricsSink = setting.MetricsSink;
            FileMetricsSinkRetention = setting.FileMetricsSinkRetention;
            FileMetricsExportPath = setting.FileMetricsExportPath;
            FileMetricsExportIntervalMs = setting.FileMetricsExportIntervalMs;
        }
    }
}
