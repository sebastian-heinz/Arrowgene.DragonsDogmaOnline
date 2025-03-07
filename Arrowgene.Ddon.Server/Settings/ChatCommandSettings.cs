using Arrowgene.Ddon.Server.Scripting.utils;
using System.ComponentModel;

namespace Arrowgene.Ddon.Server.Settings
{
    public class ChatCommandSettings : IGameSettings
    {
        public ChatCommandSettings(ScriptableSettings settingsData) : base(settingsData, typeof(ChatCommandSettings).Name)
        {
        }

        /// <summary>
        /// If set to true, disables the account type checks for all chat commands.
        /// </summary>
        [DefaultValue(_DisableAccountTypeCheck)]
        public bool DisableAccountTypeCheck
        {
            set
            {
                SetSetting("DisableAccountTypeCheck", value);
            }
            get
            {
                return TryGetSetting("DisableAccountTypeCheck", _DisableAccountTypeCheck);
            }
        }
        private const bool _DisableAccountTypeCheck = false;
    }
}
