using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftGetCraftProgressListHandler : GameRequestPacketHandler<C2SCraftGetCraftProgressListReq, S2CCraftGetCraftProgressListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftGetCraftProgressListHandler));

        public CraftGetCraftProgressListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCraftGetCraftProgressListRes Handle(GameClient client, C2SCraftGetCraftProgressListReq request)
        {
            S2CCraftGetCraftProgressListRes res = new S2CCraftGetCraftProgressListRes();
            HashSet<uint> createdRecipes = new();

            foreach (Pawn pawn in client.Character.Pawns)
            {
                res.CraftMyPawnList.Add(new CDataCraftPawnList()
                {
                    PawnId = pawn.PawnId,
                    CraftExp = pawn.CraftData.CraftExp,
                    CraftPoint = pawn.CraftData.CraftPoint,
                    CraftRankLimit = pawn.CraftData.CraftRankLimit
                });

                CraftProgress? craftProgress = Server.Database.SelectPawnCraftProgress(client.Character.CharacterId, pawn.PawnId);
                if (craftProgress != null)
                {
                    CDataCraftPawnInfo leadPawn = GetPawnCraftInfo(pawn.PawnId);
                    List<CDataCraftPawnInfo> supportPawns = new List<CDataCraftPawnInfo>();
                    if (craftProgress.CraftSupportPawnId1 > 0)
                    {
                        supportPawns.Add(GetPawnCraftInfo(craftProgress.CraftSupportPawnId1));
                    }

                    if (craftProgress.CraftSupportPawnId2 > 0)
                    {
                        supportPawns.Add(GetPawnCraftInfo(craftProgress.CraftSupportPawnId2));
                    }

                    if (craftProgress.CraftSupportPawnId3 > 0)
                    {
                        supportPawns.Add(GetPawnCraftInfo(craftProgress.CraftSupportPawnId3));
                    }

                    CDataCraftProgress CDataCraftProgress = new CDataCraftProgress()
                    {
                        CraftMainPawnInfo = leadPawn,
                        CraftSupportPawnInfoList = supportPawns,
                        CraftMasterLegendPawnInfoList = new List<CDataCraftPawnInfo>(),
                        RecipeId = craftProgress.RecipeId,
                        Exp = craftProgress.Exp,
                        NpcActionId = craftProgress.NpcActionId,
                        ItemId = craftProgress.ItemId,
                        ToppingId = 0,
                        AdditionalStatusId = craftProgress.AdditionalStatusId,
                        RemainTime = craftProgress.RemainTime,
                        ExpBonus = craftProgress.ExpBonus,
                        CreateCount = craftProgress.CreateCount
                    };
                    // Number of elements determines number icon pop up on Production Status 
                    res.CraftProgressList.Add(CDataCraftProgress);
                    if (craftProgress.RemainTime == 0)
                    {
                        createdRecipes.Add(CDataCraftProgress.RecipeId);
                    }
                    else
                    {
                        // TODO: this needs to be sent by some background thread which periodically deducts time => triggers mypawn/progress req/res, can infinitely loop if sent at the wrong time
                        craftProgress.RemainTime = 0;
                        Server.Database.UpdatePawnCraftProgress(craftProgress);
                        client.Send(new S2CCraftFinishCraftNtc { PawnId = leadPawn.PawnId });
                    }
                }
                else
                {
                    // Sanity check: if a pawn has no craft progress it should not be in crafting state
                    if (pawn.PawnState == PawnState.Craft)
                    {
                        Logger.Debug($"Resetting pawn state of pawn ID:{pawn.PawnId} because it is stuck crafting.");
                        // Something went wrong while cleaning up pawn state, handle it now
                        pawn.PawnState = PawnState.None;
                    }
                }
            }

            // Furniture can only be crafted once.
            createdRecipes.UnionWith(Server.AssetRepository.CraftingRecipesAsset
                    .Where(recipes => recipes.Category == RecipeCategory.Furniture)
                    .SelectMany(recipes => recipes.RecipeList)
                    .Where(recipe => client.Character.UnlockableItems.Contains((UnlockableItemCategory.FurnitureItem, recipe.ItemID)))
                    .Select(recipe => recipe.RecipeID));

            // Hopefully this is not super slow or pushes up against the packet limit.
            foreach (var item in client.Character.AchievementUniqueCrafts.Values.SelectMany(x => x))
            {
                var itemInfo = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, (uint)item);
                createdRecipes.UnionWith(Server.AssetRepository.CraftingRecipesAsset
                    .Where(x => x.Category == itemInfo?.RecipeCategory)
                    .SelectMany(x => x.RecipeList)
                    .Where(recipe => recipe.ItemID == (uint)item)
                    .Select(x => x.RecipeID));
            }

            res.CreatedRecipeList.AddRange(createdRecipes.Select(x => new CDataCommonU32(x)));

            return res;
        }

        private CDataCraftPawnInfo GetPawnCraftInfo(uint pawnId)
        {
            Pawn supportPawn = Server.Database.SelectPawn(pawnId);
            return new CDataCraftPawnInfo()
            {
                PawnId = supportPawn.PawnId,
                Name = supportPawn.Name
            };
        }
    }
}
