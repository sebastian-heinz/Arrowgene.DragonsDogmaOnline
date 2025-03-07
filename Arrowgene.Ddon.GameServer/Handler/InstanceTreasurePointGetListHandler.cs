using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceTreasurePointGetListHandler : GameRequestPacketHandler<C2SInstanceTreasurePointGetListReq, S2CInstanceTreasurePointGetListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceTreasurePointGetListHandler));

        public InstanceTreasurePointGetListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CInstanceTreasurePointGetListRes Handle(GameClient client, C2SInstanceTreasurePointGetListReq request)
        {
            // TODO: Implement.
            return new()
            {
                CategoryId = request.CategoryId
            };
        }
    }
}
