using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SeasonDungeonGetIdFromNpcIdHandler : GameRequestPacketHandler<C2SSeasonDungeonGetIdFromNpcIdReq, S2CSeasonDungeonGetIdFromNpcIdRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SeasonDungeonGetIdFromNpcIdHandler));

        public SeasonDungeonGetIdFromNpcIdHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSeasonDungeonGetIdFromNpcIdRes Handle(GameClient client, C2SSeasonDungeonGetIdFromNpcIdReq request)
        {
            var result = new S2CSeasonDungeonGetIdFromNpcIdRes();

            var dungeonInfo = Server.EpitaphRoadManager.GetDungeonInfo(request.NpcId);
            if (dungeonInfo != null)
            {
                result.DungeonId = dungeonInfo.DungeonId;
            }
            return result;
        }
    }
}
