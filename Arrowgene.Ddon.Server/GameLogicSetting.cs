using System.Collections.Generic;
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
        /// Must be a non-negtive value. If it is less than 0.0, a default of 1.0
        /// will be selected.
        /// </summary>
        [DataMember(Order = 3)]
        public double RookiesRingBonus { get; set; }

        /// <summary>
        /// Controls whether to pass lobby context packets on demand or only on entry to the server.
        /// True = Server entry only. Lower packet load, but also causes invisible people in lobbies.
        /// False = On-demand. May cause performance issues due to packet load.
        /// </summary>
        [DataMember(Order = 4)]
        public bool NaiveLobbyContextHandling { get; set; }

        /// <summary>
        /// Determines the maximum amount of consumable items that can be crafted in one go with a pawn.
        /// The default is a value of 10 which is equivalent to the original game's behavior.
        /// </summary>
        [DataMember(Order = 5)] public byte CraftConsumableProductionTimesMax { get; set; }

        /// <summary>
        /// Configures if party exp is adjusted based on level differences of members.
        /// </summary>
        [DataMember(Order = 6)] public bool AdjustPartyEnemyExp { get; set; }

        /// <summary>
        /// List of the inclusive ranges of (minlv, maxlv, ExpMultiplier). ExpMultiplier is a value
        /// from (0.0 - 1.0) which is multipled into the base exp amount to determine the adjusted exp.
        /// The minlv and maxlv determine the relative level range that this multiplier should be applied to.
        /// </summary>
        [DataMember(Order = 7)] public List<(uint, uint, double)> AdjustPartyEnemyExpTiers { get; set; }

        /// <summary>
        /// Configures if exp is adjusted based on level differences of members vs target level.
        /// </summary>
        [DataMember(Order = 8)] public bool AdjustTargetLvEnemyExp { get; set; }

        /// <summary>
        /// List of the inclusive ranges of (minlv, maxlv, ExpMultiplier). ExpMultiplier is a value from
        /// (0.0 - 1.0) which is multipled into the base exp amount to determine the adjusted exp.
        /// The minlv and maxlv determine the relative level range that this multiplier should be applied to.
        /// </summary>
        [DataMember(Order = 9)] public List<(uint, uint, double)> AdjustTargetLvEnemyExpTiers { get; set; }

        /// <summary>
        /// The number of real world minutes that make up an in-game day.
        /// </summary>
        [DataMember(Order = 10)] public uint GameClockTimescale { get; set; }

        /// <summary>
        /// Use a poisson process to randomly generate a weather cycle containing this many events, using the statistics in WeatherStatistics.
        /// </summary>
        [DataMember(Order = 11)] public uint WeatherSequenceLength { get; set; }

        /// <summary>
        /// Statistics that drive semirandom weather generation. List is expected to be in (Fair, Cloudy, Rainy) order.
        /// meanLength: Average length of the weather, in seconds, when it gets rolled.
        /// weight: Relative weight of rolling that weather. Set to 0 to disable.
        /// </summary>
        [DataMember(Order = 12)] public List<(uint meanLength, uint weight)> WeatherStatistics { get; set; }

        public GameLogicSetting()
        {
            AdditionalProductionSpeedFactor = 1.0;
            AdditionalCostPerformanceFactor = 1.0;
            RookiesRingMaxLevel = 89;
            RookiesRingBonus = 1.0;
            NaiveLobbyContextHandling = true;
            CraftConsumableProductionTimesMax = 10;

            AdjustPartyEnemyExp = true;
            AdjustPartyEnemyExpTiers = new List<(uint, uint, double)>()
            {
                (0, 2, 1.0),
                (3, 4, 0.9),
                (5, 6, 0.8),
                (7, 8, 0.6),
                (9, 10, 0.5),
            };

            AdjustTargetLvEnemyExp = true;
            AdjustTargetLvEnemyExpTiers = new List<(uint, uint, double)>()
            {
                (0, 2, 1.0),
                (3, 4, 0.9),
                (5, 6, 0.8),
                (7, 8, 0.6),
                (9, 10, 0.5),
            };

            GameClockTimescale = 90;

            WeatherSequenceLength = 20;
            WeatherStatistics = new List<(uint meanLength, uint weight)>
            {
                (60 * 30, 1), //Fair
                (60 * 30, 1), //Cloudy
                (60 * 30, 1), //Rainy
            };
        }

        public GameLogicSetting(GameLogicSetting setting)
        {
            AdditionalProductionSpeedFactor = setting.AdditionalProductionSpeedFactor;
            AdditionalCostPerformanceFactor = setting.AdditionalCostPerformanceFactor;
            RookiesRingMaxLevel = setting.RookiesRingMaxLevel;
            RookiesRingBonus = setting.RookiesRingBonus;
            NaiveLobbyContextHandling = setting.NaiveLobbyContextHandling;
            CraftConsumableProductionTimesMax = setting.CraftConsumableProductionTimesMax;
            AdjustPartyEnemyExp = setting.AdjustPartyEnemyExp;
            AdjustPartyEnemyExpTiers = setting.AdjustPartyEnemyExpTiers;
            AdjustTargetLvEnemyExp = setting.AdjustTargetLvEnemyExp;
            AdjustTargetLvEnemyExpTiers = setting.AdjustTargetLvEnemyExpTiers;
            GameClockTimescale = setting.GameClockTimescale;
            WeatherSequenceLength = setting.WeatherSequenceLength;
            WeatherStatistics = setting.WeatherStatistics;
        }

        // Note: method is called after the object is completely deserialized - constructors are skipped.
        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            if (RookiesRingBonus < 0)
            {
                RookiesRingBonus = 1.0;
            }
            if (AdditionalProductionSpeedFactor < 0)
            {
                CraftConsumableProductionTimesMax = 1;
            }
            if (AdditionalCostPerformanceFactor < 0)
            {
                CraftConsumableProductionTimesMax = 1;
            }
            if (CraftConsumableProductionTimesMax < 1)
            {
                CraftConsumableProductionTimesMax = 10;
            }
            if (GameClockTimescale <= 0)
            {
                GameClockTimescale = 90;
            }
        }
    }
}
