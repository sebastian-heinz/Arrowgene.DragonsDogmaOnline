using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SeasonDungeonGetInfoHandler : GameRequestPacketHandler<C2SSeasonDungeonGetInfoReq, S2CSeasonDungeonGetInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SeasonDungeonGetInfoHandler));

        public SeasonDungeonGetInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSeasonDungeonGetInfoRes Handle(GameClient client, C2SSeasonDungeonGetInfoReq request)
        {
            var result = new S2CSeasonDungeonGetInfoRes();

            var dungeonInfo = Server.EpitaphRoadManager.GetDungeonInfo(request.DungeonId);

            result.DungeonInfo.Unk0 = 40; // Unsure what 40 does, was in packet capture
            result.DungeonInfo.Name = dungeonInfo.Name;
            result.DungeonInfo.DungeonId = dungeonInfo.DungeonId;

            foreach (var section in dungeonInfo.Sections)
            {
                if ((section.UnlockCost.Count == 0 && section.BarrierDependencies.Count == 0) ||
                    client.Character.EpitaphRoadState.UnlockedContent.Contains(section.EpitaphId))
                {
                    result.DungeonSections.Add(section.AsCDataSeasonDungeonSection());
                }
            }

            return result;
        }
    }
}
