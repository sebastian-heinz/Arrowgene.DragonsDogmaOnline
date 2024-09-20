using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ServerGetGameSettingHandler : GameRequestPacketHandler<C2SServerGetGameSettingReq, S2CServerGetGameSettingRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ServerGetGameSettingHandler));

        public ServerGetGameSettingHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CServerGetGameSettingRes Handle(GameClient client, C2SServerGetGameSettingReq request)
        {
            var res = new S2CServerGetGameSettingRes.Serializer().Read(GameDump.Dump_10.AsBuffer());

            return res;
        }
    }
}
