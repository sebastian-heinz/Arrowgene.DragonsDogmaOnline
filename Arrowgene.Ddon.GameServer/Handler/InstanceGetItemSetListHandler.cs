using Arrowgene.Ddon.GameServer.GatheringItems.Generators;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetItemSetListHandler : GameRequestPacketHandler<C2SInstanceGetItemSetListReq, S2CInstanceGetItemSetListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetItemSetListHandler));

        public InstanceGetItemSetListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CInstanceGetItemSetListRes Handle(GameClient client, C2SInstanceGetItemSetListReq request)
        {
            S2CInstanceGetItemSetListRes res = new()
            {
                LayoutId = request.LayoutId
            };

            // If you don't send the corresponding position, that gathering point doesn't appear.
            // Figuring out what spots are valid is somewhat tricky (spread across multiple subsystems)
            // so just return all possible spots and call it good. This is what we were doing with the old handler.
            HashSet<byte> posIds = Enumerable.Range(0, byte.MaxValue).Select(x => (byte)x).ToHashSet();
            HashSet<byte> emptySpots = new();

            // Filter out one off gathering spots (red shinies)
            var stage = Stage.StageInfoFromStageLayoutId(request.LayoutId.AsStageLayoutId());
            if (OneOffGatheringItemGenerator.OneOffGatherType.TryGetValue(stage, out var unlockableItemCategory))
            {
                if (client.Character.HasContentReleased(ContentsRelease.AreaInvestigation))
                {
                    // Mark consumed red shinies as empty.
                    var foundShinies = Server.AssetRepository.GatheringSpotInfoAsset.GatheringInfoMap
                    .GetValueOrDefault(stage.StageNo)
                    .Where(x => x.Key.GroupNo == request.LayoutId.GroupId
                        && x.Value.GatheringType == GatheringType.OM_GATHER_ONE_OFF
                        && client.Character.UnlockableItems.Contains((unlockableItemCategory, x.Key.GroupNo * 100 + x.Key.PosId))
                    )
                    .Select(x => (byte)x.Key.PosId)
                    .ToHashSet();

                    emptySpots.UnionWith(foundShinies);
                }
                else
                {
                    // Hide red shinies entirely until you've released them.
                    var allShinies = Server.AssetRepository.GatheringSpotInfoAsset.GatheringInfoMap
                    .GetValueOrDefault(stage.StageNo)
                    .Where(x => x.Key.GroupNo == request.LayoutId.GroupId
                        && x.Value.GatheringType == GatheringType.OM_GATHER_ONE_OFF
                    )
                    .Select(x => (byte)x.Key.PosId)
                    .ToHashSet();

                    // Special case to show a shiny early
                    if (client.QuestState.GetActiveQuestScheduleIds().Contains(60215006) 
                        && stage == Stage.FaranaPlains0
                        && request.LayoutId.GroupId == 50
                    )
                    {
                        allShinies.Remove(7); // Shiny next to the Arisen Corps Regiment Soldier
                    }

                    posIds.ExceptWith(allShinies);
                }
            }

            res.SetList = posIds.Select(x => new CDataLayoutItemData()
            {
                PosId = x,
                IsEmpty = emptySpots.Contains(x)
            }).ToList();

            return res;
        }
    }
}
