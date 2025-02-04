using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SeasonDungeonGetBlockadeIdFromNpcIdHandler : GameRequestPacketHandler<C2SSeasonDungeonGetBlockadeIdFromNpcIdReq, S2CSeasonDungeonGetBlockadeIdFromNpcIdRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SeasonDungeonGetBlockadeIdFromNpcIdHandler));

        public SeasonDungeonGetBlockadeIdFromNpcIdHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSeasonDungeonGetBlockadeIdFromNpcIdRes Handle(GameClient client, C2SSeasonDungeonGetBlockadeIdFromNpcIdReq request)
        {
            return new S2CSeasonDungeonGetBlockadeIdFromNpcIdRes()
            {
                EpitaphId = Server.EpitaphRoadManager.GetBarrier(request.NpcId).EpitaphId 
            };
        }
    }
}
