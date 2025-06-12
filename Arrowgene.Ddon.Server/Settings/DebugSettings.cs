using Arrowgene.Ddon.Server.Scripting.utils;
using Arrowgene.Ddon.Shared.Model.Quest;
using System.Collections.Generic;
using System.ComponentModel;

namespace Arrowgene.Ddon.Server.Settings
{
    public class DebugSettings : IGameSettings
    {
        public DebugSettings(ScriptableSettings settingsData) : base(settingsData, typeof(DebugSettings).Name)
        {
        }

        /// <summary>
        /// Used when debugging requires a hot reloadable quest id.
        /// </summary>
        [DefaultValue(_QuestId)]
        public QuestId QuestId
        {
            set
            {
                SetSetting("QuestId", value);
            }
            get
            {
                return TryGetSetting("QuestId", _QuestId);
            }
        }
        private const QuestId _QuestId = 0;

        /// <summary>
        /// Used when debugging requires a hot reloadable list of uints
        /// </summary>
        [DefaultValue("new List<uint>() {}")]
        public List<uint> UintList
        {
            set
            {
                SetSetting("UintList", value);
            }
            get
            {
                return TryGetSetting<List<uint>>("UintList", []);
            }
        }
    }
}
