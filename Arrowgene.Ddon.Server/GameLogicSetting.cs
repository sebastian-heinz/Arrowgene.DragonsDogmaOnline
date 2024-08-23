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

        /// <summary>
        /// Sets the maximim level that the exp ring will reward a bonus.
        /// </summary>
        [DataMember(Order = 2)]
        public uint RookiesRingMaxLevel { get; set; }

        /// <summary>
        /// The multiplier applied to the bonus amount of exp rewarded.
        /// Must be a non-negtive value. If it is < 0.0, a default of 1.0
        /// will be selected.
        /// </summary>
        [DataMember(Order = 3)]
        public double RookiesRingBonus { get; set; }

        public GameLogicSetting()
        {
            AdditionalProductionSpeedFactor = 1.0;
            AdditionalCostPerformanceFactor = 1.0;
            RookiesRingMaxLevel = 89;
            RookiesRingBonus = 1.0;
        }

        public GameLogicSetting(GameLogicSetting setting)
        {
            AdditionalProductionSpeedFactor = setting.AdditionalProductionSpeedFactor;
            AdditionalCostPerformanceFactor = setting.AdditionalCostPerformanceFactor;
            RookiesRingMaxLevel = setting.RookiesRingMaxLevel;
            RookiesRingBonus = setting.RookiesRingBonus;
        }

        // Note: method is called after the object is completely deserialized - constructors are skipped.
        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            if (RookiesRingBonus < 0)
            {
                RookiesRingBonus = 1.0;
            }
        }
    }
}
