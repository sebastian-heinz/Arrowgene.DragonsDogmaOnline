using System.Runtime.Serialization;

namespace Arrowgene.Ddon.Server
{
    [DataContract]
    public class GameLogicSetting
    {
        /// <summary>
        /// Additional factor to change how long crafting a recipe will take to finish.
        /// </summary>
        [DataMember(Order = 0)]
        public double AdditionalProductionSpeedFactor { get; set; }

        /// <summary>
        /// Additional factor to change how much a recipe will cost.
        /// </summary>
        [DataMember(Order = 1)]
        public double AdditionalCostPerformanceFactor { get; set; }

        public byte CraftConsumableProductionTimesMax { get; set; } = 10;

        public GameLogicSetting()
        {
            AdditionalProductionSpeedFactor = 1.0;
            AdditionalCostPerformanceFactor = 1.0;
            CraftConsumableProductionTimesMax = 10;
        }

        public GameLogicSetting(GameLogicSetting setting)
        {
            AdditionalProductionSpeedFactor = setting.AdditionalProductionSpeedFactor;
            AdditionalCostPerformanceFactor = setting.AdditionalCostPerformanceFactor;
            CraftConsumableProductionTimesMax = setting.CraftConsumableProductionTimesMax;
        }

        // Note: method is called after the object is completely deserialized - constructors are skipped.
        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
        }
    }
}
