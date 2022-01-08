using System.Runtime.Serialization;
using Arrowgene.Ddon.Server;

namespace Arrowgene.Ddon.LoginServer
{
    [DataContract]
    public class LoginServerSetting
    {
        [DataMember(Order = 1)] public ServerSetting ServerSetting { get; set; }

        public LoginServerSetting()
        {
            ServerSetting = new ServerSetting();
            ServerSetting.ServerPort = 52100;
            ServerSetting.Name = "Login";
        }

        public LoginServerSetting(LoginServerSetting setting)
        {
            ServerSetting = new ServerSetting(setting.ServerSetting);
        }
    }
}
