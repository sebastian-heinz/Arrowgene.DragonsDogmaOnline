using Arrowgene.Buffers;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ActionSetPlayerActionHistoryHandler : StructurePacketHandler<GameClient, C2SActionSetPlayerActionHistoryReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ActionSetPlayerActionHistoryHandler));

        public ActionSetPlayerActionHistoryHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SActionSetPlayerActionHistoryReq> packet)
        {
            S2CActionSetPlayerActionHistoryRes res = new S2CActionSetPlayerActionHistoryRes();
            client.Send(res);
        }
    }
}