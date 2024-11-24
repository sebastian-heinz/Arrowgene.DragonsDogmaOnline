using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SeasonDungeonGetExRequiredItemHandler : GameRequestPacketHandler<C2SSeasonDungeonGetExRequiredItemReq, S2CSeasonDungeonGetExRequiredItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SeasonDungeonGetExRequiredItemHandler));

        public SeasonDungeonGetExRequiredItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSeasonDungeonGetExRequiredItemRes Handle(GameClient client, C2SSeasonDungeonGetExRequiredItemReq request)
        {
            var result = new S2CSeasonDungeonGetExRequiredItemRes()
            {
                ItemList = Server.EpitaphRoadManager.GetCostByEpitahId(request.EpitaphId)
            };
            return result;
        }
    }
}
