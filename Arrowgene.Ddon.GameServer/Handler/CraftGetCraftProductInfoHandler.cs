using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftGetCraftProductInfoHandler : GameRequestPacketHandler<C2SCraftGetCraftProductInfoReq, C2SCraftGetCraftProductInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftGetCraftProductInfoHandler));

        public CraftGetCraftProductInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override C2SCraftGetCraftProductInfoRes Handle(GameClient client, C2SCraftGetCraftProductInfoReq request)
        {
            C2SCraftGetCraftProductInfoRes craftProductInfoRes = new C2SCraftGetCraftProductInfoRes();

            CraftProgress craftProgress = Server.Database.SelectPawnCraftProgress(client.Character.CharacterId, request.CraftMainPawnID);
            CDataCraftProductInfo craftProductInfo = new CDataCraftProductInfo()
            {
                ItemID = craftProgress.ItemId,
                ItemNum = craftProgress.CreateCount,
                Unk0 = craftProgress.AdditionalStatusId,
                PlusValue = (byte)craftProgress.PlusValue,
                Exp = craftProgress.Exp,
                ExtraBonus = craftProgress.BonusExp,
                IsGreatSuccess = craftProgress.GreatSuccess
            };

            // The lead pawn can only be a pawn owned by the player, no need to search in DB.
            Pawn leadPawn = Server.CraftManager.FindPawn(client, request.CraftMainPawnID);
            if (CraftManager.IsCraftRankLimitPromotionRecipe(leadPawn, craftProgress.RecipeId))
            {
                CraftManager.PromotePawnRankLimit(leadPawn);
                // Mandatory to send otherwise the UI gets stuck.
                CraftManager.HandlePawnExpUpNtc(client, leadPawn, 0, 0);
                // TODO: This is not accurate to the original game but currently there is no other way to gain crafting reset points.
                Server.WalletManager.AddToWalletNtc(client, client.Character, WalletType.ResetCraftSkills, 1, 0, ItemNoticeType.ResetCraftpoint);
                Server.Database.UpdatePawnBaseInfo(leadPawn);
            }
            else
            {
                if (CraftManager.CanPawnExpUp(leadPawn))
                {
                    double BonusExpMultiplier = Server.GpCourseManager.PawnCraftBonus();
                    CraftManager.HandlePawnExpUpNtc(client, leadPawn, craftProgress.Exp, BonusExpMultiplier);
                    if (CraftManager.CanPawnRankUp(leadPawn))
                    {
                        CraftManager.HandlePawnRankUpNtc(client, leadPawn);
                    }

                    Server.Database.UpdatePawnBaseInfo(leadPawn);
                }
                else
                {
                    // Mandatory to send otherwise the UI gets stuck.
                    CraftManager.HandlePawnExpUpNtc(client, leadPawn, 0, 0);
                }
            }

            // TODO: track and increase CraftCount for NTC 8.35.16
            craftProductInfoRes.CraftProductInfo = craftProductInfo;
            return craftProductInfoRes;
        }
    }
}
