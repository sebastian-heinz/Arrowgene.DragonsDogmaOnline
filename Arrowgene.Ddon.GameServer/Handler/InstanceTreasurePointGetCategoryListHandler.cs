using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceTreasurePointGetCategoryListHandler : GameRequestPacketHandler<C2SInstanceTreasurePointGetCategoryListReq, S2CInstanceTreasurePointGetCategoryListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceTreasurePointGetCategoryListHandler));

        public InstanceTreasurePointGetCategoryListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CInstanceTreasurePointGetCategoryListRes Handle(GameClient client, C2SInstanceTreasurePointGetCategoryListReq request)
        {
            // TODO: Implement.
            // This is seemingly related to the lore entries "'Trades of a Explorer' that serves as a clue to the Clan dungeon in DDON"
            return new();
        }
    }
}
