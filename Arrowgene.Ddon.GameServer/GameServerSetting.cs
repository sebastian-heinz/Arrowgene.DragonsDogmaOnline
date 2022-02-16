using System.IO;
using System.Runtime.Serialization;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;

namespace Arrowgene.Ddon.GameServer
{
    [DataContract]
    public class GameServerSetting
    {
        [DataMember(Order = 1)] public ServerSetting ServerSetting { get; set; }

        public GameServerSetting()
        {
            ServerSetting = new ServerSetting();
            ServerSetting.ServerPort = 52000;
            ServerSetting.Name = "Game";
        }

        public GameServerSetting(GameServerSetting setting)
        {
            ServerSetting = new ServerSetting(setting.ServerSetting);
        }
    }
}
