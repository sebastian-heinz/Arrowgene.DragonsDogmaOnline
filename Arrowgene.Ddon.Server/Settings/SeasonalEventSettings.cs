using Arrowgene.Ddon.Server.Scripting;
using Arrowgene.Ddon.Server.Scripting.utils;
using System;
using System.ComponentModel;

namespace Arrowgene.Ddon.Server.Settings
{
    public class SeasonalEventSettings : IGameSettings
    {
        public SeasonalEventSettings(ScriptableSettings settingsData) : base(settingsData, typeof(SeasonalEventSettings).Name)
        {
        }

        /// <summary>
        /// Used to determine if the Halloween seasonal event is enabled or not.
        /// </summary>
        [DefaultValue(_EnableHalloweenEvent)]
        public bool EnableHalloweenEvent
        {
            set
            {
                SetSetting("EnableHalloweenEvent", value);
            }
            get
            {
                return TryGetSetting("EnableHalloweenEvent", _EnableHalloweenEvent);
            }
        }
        private const bool _EnableHalloweenEvent = true;

        /// <summary>
        /// The daterange that the Halloween event should be available
        /// if EnableHalloweenEvent is set to true. The format is in MM/DD.
        /// </summary>
        [DefaultValue("LibUtils.EventTimespan(\"10/1\", \"10/31\")")]
        public (DateTime StartDate, DateTime EndDate) HalloweenValidPeriod
        {
            set
            {
                SetSetting("HalloweenValidPeriod", value);
            }
            get
            {
                return TryGetSetting("HalloweenValidPeriod", LibUtils.EventTimespan("10/1", "10/31"));
            }
        }

        /// <summary>
        /// This option configures which version will be used when
        /// the setting EnableHaloweenEvent is set to true.
        ///
        /// 2016 (Not implemented)
        /// - Light a Pumpkin Lantern? (1)
        /// - Light a Pumpkin Lantern? (2)
        /// 
        /// 2017
        /// - The Darkness of Halloween
        /// 
        /// 2018
        /// - Emergency! Not Enough Candy (1)
        /// - Emergency! Not Enough Candy (2)
        /// </summary>
        [DefaultValue(_HalloweenEventYear)]
        public uint HalloweenEventYear
        {
            set
            {
                SetSetting("HalloweenEventYear", value);
            }
            get
            {
                return TryGetSetting("HalloweenEventYear", _HalloweenEventYear);
            }
        }
        private const uint _HalloweenEventYear = 2018;

        /// <summary>
        /// Used to determine if the Christmas Seasonal event is enabled or not.
        /// </summary>
        [DefaultValue(_EnableChristmasEvent)]
        public bool EnableChristmasEvent
        {
            set
            {
                SetSetting<bool>("EnableChristmasEvent", value);
            }
            get
            {
                return TryGetSetting<bool>("EnableChristmasEvent", _EnableChristmasEvent);
            }
        }
        private const bool _EnableChristmasEvent = true;

        /// <summary>
        /// The daterange that the Christmas event should be available
        /// if EnableChristmasEvent is set to true. The format is in MM/DD.
        /// </summary>
        [DefaultValue("LibUtils.EventTimespan(\"12/1\", \"12/31\")")]
        public (DateTime StartDate, DateTime EndDate) ChristmasValidPeriod
        {
            set
            {
                SetSetting("ChristmasValidPeriod", value);
            }
            get
            {
                return TryGetSetting("ChristmasValidPeriod", LibUtils.EventTimespan("12/1", "12/31"));
            }
        }

        /// <summary>
        /// This option configures which version will be used when
        /// the setting EnableChristmasEvent is set to true.
        /// 
        /// 2016 (Not implemented)
        /// - Path to Miracles (1)
        /// - Path to Miracles (2)
        /// - Path to Miracles (3)
        /// - Path to Miracles (4)
        /// - With GreatDesire to Gift: Upper
        /// - With GreatDesire to Gift: Lower
        /// 
        /// 2017 (Not implemented)
        /// - Wish on a Shooting Star (1)
        /// - Wish on a Shooting Star (2)
        /// - Wish on a Shooting Star (3)
        /// - Wish on a Shooting Star (4)
        /// 
        /// 2018
        /// - Merry Christmas with Smiles (1)
        /// - Merry Christmas with Smiles (2)
        /// </summary>
        [DefaultValue(_ChristmasEventYear)]
        public uint ChristmasEventYear
        {
            set
            {
                SetSetting("ChristmasEventYear", value);
            }
            get
            {
                return TryGetSetting("ChristmasEventYear", _ChristmasEventYear);
            }
        }
        private const uint _ChristmasEventYear = 2018;

        /// <summary>
        /// Used to determine if the Valentines Seasonal event is enabled or not.
        /// </summary>
        [DefaultValue(_EnableValentinesEvent)]
        public bool EnableValentinesEvent
        {
            set
            {
                SetSetting("EnableValentinesEvent", value);
            }
            get
            {
                return TryGetSetting("EnableValentinesEvent", _EnableValentinesEvent);
            }
        }
        private const bool _EnableValentinesEvent = true;

        /// <summary>
        /// The daterange that the Valentines event should be available
        /// if EnableValentinesEvent is set to true. The format is in MM/DD.
        /// </summary>
        [DefaultValue("LibUtils.EventTimespan(\"2/14\", \"2/29\")")]
        public (DateTime StartDate, DateTime EndDate) ValentinesValidPeriod
        {
            set
            {
                SetSetting("ValentinesValidPeriod", value);
            }
            get
            {
                return TryGetSetting("ValentinesValidPeriod", LibUtils.EventTimespan("2/14", "2/29"));
            }
        }

        /// <summary>
        /// This option configures which version will be used when
        /// the setting EnableValentinesEvent is set to true.
        /// 
        /// 2017
        /// - Shape of Love for Someone (1)
        /// - Shape of Love for Someone (2)
        /// 
        /// 2018 (Not implememted)
        /// - All Your Feelings in One Glance (1)
        /// - All Your Feelings in One Glance (2)
        /// </summary>
        [DefaultValue(_ValentinesEventYear)]
        public uint ValentinesEventYear
        {
            set
            {
                SetSetting("ValentinesEventYear", value);
            }
            get
            {
                return TryGetSetting("ValentinesEventYear", _ValentinesEventYear);
            }
        }
        private const uint _ValentinesEventYear = 2017;
    }
}
