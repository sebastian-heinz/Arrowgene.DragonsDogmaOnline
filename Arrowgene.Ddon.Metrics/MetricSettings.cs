using System.Runtime.Serialization;

namespace Arrowgene.Ddon.Metrics
{
    [DataContract]
    public class MetricSettings
    {
        [DataMember(Order = 1)] public bool Enabled { get; set; }
        [DataMember(Order = 10)] public MetricsSinkType MetricsSink { get; set; }
        [DataMember(Order = 20)] public int SamplingIntervalMs { get; set; }
        [DataMember(Order = 50)] public long FileMetricsSinkRetentionMin { get; set; }
        [DataMember(Order = 51)] public string FileMetricsExportPath { get; set; }
        [DataMember(Order = 52)] public int FileMetricsExportIntervalMs { get; set; }


        public MetricSettings()
        {
            Enabled = false;
            MetricsSink = MetricsSinkType.FileMetricsSink;
            SamplingIntervalMs = 1000;
            FileMetricsSinkRetentionMin = 60 * 24;
            FileMetricsExportPath = "Files/www/metrics/snapshot";
            FileMetricsExportIntervalMs = 30000;
        }

        public MetricSettings(MetricSettings setting)
        {
            Enabled = setting.Enabled;
            MetricsSink = setting.MetricsSink;
            FileMetricsSinkRetentionMin = setting.FileMetricsSinkRetentionMin;
            FileMetricsExportPath = setting.FileMetricsExportPath;
            FileMetricsExportIntervalMs = setting.FileMetricsExportIntervalMs;
        }
    }
}
