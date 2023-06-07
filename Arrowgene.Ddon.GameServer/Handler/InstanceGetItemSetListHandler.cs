using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetItemSetListHandler : StructurePacketHandler<GameClient, C2SInstanceGetItemSetListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetItemSetListHandler));


        public InstanceGetItemSetListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceGetItemSetListReq> req)
        {
            S2CInstanceGetItemSetListRes res = new S2CInstanceGetItemSetListRes(req.Structure);
            client.Send(res);
        }
    }
}