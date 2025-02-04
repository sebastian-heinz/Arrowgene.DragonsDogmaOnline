using System.Runtime.Serialization;

namespace Arrowgene.Ddon.Server
{
    [DataContract]
    public class LoginServerSetting
    {
        [DataMember(Order = 1)] public ServerSetting ServerSetting { get; set; }
        [DataMember(Order = 90)] public bool AccountRequired { get; set; }
        [DataMember(Order = 105)] public uint NoOperationTimeOutTime { get; set; }
        [DataMember(Order = 110)] public bool KickOnMultipleLogin { get; set; }
        [DataMember(Order = 111)] public int KickOnMultipleLoginTries { get; set; }
        [DataMember(Order = 112)] public int KickOnMultipleLoginTimer { get; set; }

        public LoginServerSetting()
        {
            SetDefaultValues();
        }

        public LoginServerSetting(LoginServerSetting setting)
        {
            ServerSetting = new ServerSetting(setting.ServerSetting);
            AccountRequired = setting.AccountRequired;
            NoOperationTimeOutTime = setting.NoOperationTimeOutTime;
            KickOnMultipleLogin = setting.KickOnMultipleLogin;
            KickOnMultipleLoginTries = setting.KickOnMultipleLoginTries;
            KickOnMultipleLoginTimer = setting.KickOnMultipleLoginTimer;
        }

        [OnDeserializing]
        void OnDeserializing(StreamingContext context)
        {
            SetDefaultValues();
        }

        void SetDefaultValues()
        {
            ServerSetting = new ServerSetting();
            ServerSetting.Id = 1;
            ServerSetting.Name = "Login";
            ServerSetting.ServerPort = 52100;
            ServerSetting.ServerSocketSettings.Identity = "Login";

            AccountRequired = false;
            NoOperationTimeOutTime = 14400;
            KickOnMultipleLogin = false;
            KickOnMultipleLoginTries = 3;
            KickOnMultipleLoginTimer = 5000;
        }
    }
}
