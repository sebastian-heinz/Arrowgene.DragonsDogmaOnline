using System.Collections.Generic;
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

            // TODO: unsure what this is for, "topping" sounds like the sorting in the UI?..
            uint toppingCounter = 1;
            foreach (Pawn pawn in client.Character.Pawns)
            {
                res.CraftMyPawnList.Add(new CDataCraftPawnList()
                {
                    PawnId = pawn.PawnId,
                    CraftExp = pawn.CraftData.CraftExp,
                    CraftPoint = pawn.CraftData.CraftPoint,
                    CraftRankLimit = pawn.CraftData.CraftRankLimit
                });

                CraftProgress craftProgress = Server.Database.SelectPawnCraftProgress(client.Character.CharacterId, pawn.PawnId);
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
                        ToppingId = toppingCounter++,
                        Unk0 = craftProgress.Unk0,
                        RemainTime = craftProgress.RemainTime,
                        ExpBonus = craftProgress.ExpBonus,
                        CreateCount = craftProgress.CreateCount
                    };
                    // Number of elements determines number icon pop up on Production Status 
                    res.CraftProgressList.Add(CDataCraftProgress);
                    if (craftProgress.RemainTime == 0)
                    {
                        res.CreatedRecipeList.Add(new CDataCommonU32(CDataCraftProgress.RecipeId));
                    }
                    else
                    {
                        // TODO: this needs to be sent by some background thread which periodically deducts time => triggers mypawn/progress req/res, can infinitely loop if sent at the wrong time
                        craftProgress.RemainTime = 0;
                        Server.Database.UpdatePawnCraftProgress(craftProgress);
                        client.Send(new S2CCraftFinishCraftNtc { PawnId = leadPawn.PawnId });
                    }
                }
            }

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
