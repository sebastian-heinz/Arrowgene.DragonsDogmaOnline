using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.GatheringItems.Generators
{
    public class OneOffGatheringItemGenerator : IGatheringGenerator
    {
        DdonGameServer Server;

        public OneOffGatheringItemGenerator(DdonGameServer server)
        {
            Server = server;
        }


        public override bool IsEnabled()
        {
            return true;
        }

        // TODO: Roll this into DefaultGatheringItemGenerator?

        public static readonly Dictionary<StageInfo, ItemId> ValidStages = new()
        {
            { Stage.ElanWaterGrove, ItemId.SpiritualFlame },
            { Stage.FaranaPlains0, ItemId.SpiritualFlame },
            { Stage.MorrowForest, ItemId.SpiritualFlame },
            { Stage.KingalCanyon, ItemId.SpiritualFlame },
            { Stage.RathniteFoothills, ItemId.EyeOre },
            { Stage.FeryanaWilderness, ItemId.SilverEyeOre },
            { Stage.MegadosysPlateau, ItemId.GoldenMistletoe },
        };

        public static readonly Dictionary<StageInfo, UnlockableItemCategory> OneOffGatherType = new()
        {
            { Stage.ElanWaterGrove, UnlockableItemCategory.AreaVisualSurveyElanWaterGrove },
            { Stage.FaranaPlains0, UnlockableItemCategory.AreaVisualSurveyFaranaPlains },
            { Stage.MorrowForest, UnlockableItemCategory.AreaVisualSurveyMorrowForest },
            { Stage.KingalCanyon, UnlockableItemCategory.AreaVisualSurveyKingalCanyon },
            { Stage.RathniteFoothills, UnlockableItemCategory.AreaVisualSurveyRathniteFoothills },
            { Stage.FeryanaWilderness, UnlockableItemCategory.AreaVisualSurveyFeryanaWilderness },
            { Stage.MegadosysPlateau, UnlockableItemCategory.AreaVisualSurveyMegadosysPlateau },
        };


        public override List<InstancedGatheringItem> Generate(GameClient client, StageLayoutId stageId, uint index)
        {
            var stage = Stage.StageInfoFromStageLayoutId(stageId);

            if (client.Character.HasContentReleased(ContentsRelease.AreaInvestigation) 
                && ValidStages.TryGetValue(stage, out ItemId itemId))
            {
                var stageSpots = Server.AssetRepository.GatheringSpotInfoAsset.GatheringInfoMap[stage.StageNo];

                if (stageSpots.TryGetValue((stageId.GroupId, index), out var spotInfo) && spotInfo.GatheringType == GatheringType.OM_GATHER_ONE_OFF)
                {
                    return
                    [
                        new()
                        {
                            ItemId = itemId,
                            ItemNum = 1,
                        }
                    ];
                }
                else
                {
                    return [];
                }
            }
            else if (client.QuestState.GetActiveQuestScheduleIds().Contains(60215006) // Special case to show a shiny early
                && stage == Stage.FaranaPlains0 && stageId.GroupId == 50 && index == 7)
            {
                return
                [
                    new()
                    {
                        ItemId = ItemId.SpiritualFlame,
                        ItemNum = 1,
                    }
                ];
            }
            else
            {
                return [];
            }
        }
    }
}
