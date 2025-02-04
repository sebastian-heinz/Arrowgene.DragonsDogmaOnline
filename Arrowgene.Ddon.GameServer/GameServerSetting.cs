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
            SetDefaultValues();
        }

        public GameServerSetting(GameServerSetting setting)
        {
            ServerSetting = new ServerSetting(setting.ServerSetting);
        }

        // Note: method is called after the object is completely deserialized - constructors are skipped.
        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            if(ServerSetting == null)
            {
                ServerSetting = new ServerSetting();
                ServerSetting.Id = 10;
                ServerSetting.Name = "Game";
                ServerSetting.ServerPort = 52000;
                ServerSetting.ServerSocketSettings.Identity = "Game";
            }
        }

        [OnDeserializing]
        void OnDeserializing(StreamingContext context)
        {
            SetDefaultValues();
        }

        void SetDefaultValues()
        {
            ServerSetting = new ServerSetting();
            ServerSetting.Id = 10;
            ServerSetting.Name = "Game";
            ServerSetting.ServerPort = 52000;
            ServerSetting.ServerSocketSettings.Identity = "Game";
        }
    }
}
