using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SeasonDungeonGetExtendableBlockadeListFromNpcIdHandler : GameRequestPacketHandler<C2SSeasonDungeonGetExtendableBlockadeListFromNpcIdReq, S2CSeasonDungeonGetExtendableBlockadeListFromNpcIdRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SeasonDungeonGetExtendableBlockadeListFromNpcIdHandler));

        public SeasonDungeonGetExtendableBlockadeListFromNpcIdHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSeasonDungeonGetExtendableBlockadeListFromNpcIdRes Handle(GameClient client, C2SSeasonDungeonGetExtendableBlockadeListFromNpcIdReq request)
        {
            // This list is how you unlock paths/checkpoints into the dungeon
            var result = new S2CSeasonDungeonGetExtendableBlockadeListFromNpcIdRes();

            var dungeonInfo = Server.EpitaphRoadManager.GetDungeonInfo(request.NpcId);

            foreach (var section in dungeonInfo.Sections)
            {
                if ((section.UnlockCost.Count == 0) || client.Character.EpitaphRoadState.UnlockedContent.Contains(section.EpitaphId))
                {
                    continue;
                }

                if (section.BarrierDependencies.Count > 0)
                {
                    // Barrier needs to be unlocked from inside the dungeon
                    continue;
                }

                result.BlockadeList.Add(new CDataSeasonDungeonBlockadeElement()
                {
                    EpitaphId = section.EpitaphId,
                    Name = section.Name
                });
            }

            return result;
        }
    }
}
