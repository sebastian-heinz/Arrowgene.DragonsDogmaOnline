using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class AreaGetLeaderAreaReleaseListHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(AreaGetLeaderAreaReleaseListHandler));


        public AreaGetLeaderAreaReleaseListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_AREA_GET_LEADER_AREA_RELEASE_LIST_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            client.Send(GameFull.Dump_117);
        }
    }
}
