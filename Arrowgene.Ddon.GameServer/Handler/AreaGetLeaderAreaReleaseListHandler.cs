using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class AreaGetLeaderAreaReleaseListHandler : GameRequestPacketHandler<C2SAreaGetLeaderAreaReleaseListReq, S2CAreaGetLeaderAreaReleaseListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AreaGetLeaderAreaReleaseListHandler));

        public AreaGetLeaderAreaReleaseListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CAreaGetLeaderAreaReleaseListRes Handle(GameClient client, C2SAreaGetLeaderAreaReleaseListReq request)
        {
            // client.Send(GameFull.Dump_117);
            var result = new S2CAreaGetLeaderAreaReleaseListRes.Serializer().Read(GameFull.Dump_117.AsBuffer());

            return result;
        }
    }
}
