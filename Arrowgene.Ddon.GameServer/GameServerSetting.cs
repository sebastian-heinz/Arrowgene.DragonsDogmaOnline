using System.Runtime.Serialization;
using Arrowgene.Ddon.Server;

namespace Arrowgene.Ddon.GameServer
{
    [DataContract]
    public class GameServerSetting
    {
        [DataMember(Order = 1)] public ServerSetting ServerSetting { get; set; }

        public GameServerSetting()
        {
            ServerSetting = new ServerSetting();
            ServerSetting.Id = 10;
            ServerSetting.Name = "Game";
            ServerSetting.ServerPort = 52000;
            ServerSetting.ServerSocketSettings.Identity = "Game";
        }

        public GameServerSetting(GameServerSetting setting)
        {
            ServerSetting = new ServerSetting(setting.ServerSetting);
        }
    }
}
