using System.Runtime.Serialization;

namespace Arrowgene.Ddon.Metrics
{
    [DataContract]
    public class MetricSettings
    {
        [DataMember(Order = 1)] public bool Enabled { get; set; }
        [DataMember(Order = 20)] public int SamplingIntervalMs { get; set; }
        [DataMember(Order = 10)] public MetricsSinkType MetricsSink { get; set; }
        [DataMember(Order = 20)] public long MemoryMetricsSinkRetention { get; set; }


        public MetricSettings()
        {
            Enabled = false;
            MetricsSink = MetricsSinkType.MemoryMetricsSink;
            MemoryMetricsSinkRetention = 1000;
        }

        public MetricSettings(MetricSettings setting)
        {
            Enabled = setting.Enabled;
            MetricsSink = setting.MetricsSink;
            MemoryMetricsSinkRetention = setting.MemoryMetricsSinkRetention;
        }
    }
}
