using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ServerGetGameSettingHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ServerGetGameSettingHandler));


        public ServerGetGameSettingHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_SERVER_GET_GAME_SETTING_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(GameDump.Dump_10);
        }
    }
}
