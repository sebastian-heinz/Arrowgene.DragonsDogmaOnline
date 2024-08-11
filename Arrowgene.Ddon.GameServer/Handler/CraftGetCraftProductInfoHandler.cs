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
                Unk0 = craftProgress.Unk0,
                PlusValue = (byte)craftProgress.PlusValue,
                Exp = craftProgress.Exp,
                ExtraBonus = craftProgress.BonusExp,
                IsGreatSuccess = craftProgress.GreatSuccess
            };
            craftProductInfoRes.CraftProductInfo = craftProductInfo;

            // The lead pawn can only be a pawn owned by the player, no need to search in DB.
            Pawn leadPawn = client.Character.Pawns.Find(p => p.PawnId == request.CraftMainPawnID);

            if (Server.CraftManager.IsCraftRankLimitPromotionRecipe(leadPawn, craftProgress.RecipeId))
            {
                Server.CraftManager.PromotePawnRankLimit(leadPawn);

                // TODO: This is not accurate to the original game but currently there is no other way to gain crafting reset points.
                CDataWalletPoint resetCraftSkillWalletPoint = client.Character.WalletPointList.Find(l => l.Type == WalletType.ResetCraftSkills);
                resetCraftSkillWalletPoint.Value++;
                S2CItemUpdateCharacterItemNtc itemUpdateNtc = new S2CItemUpdateCharacterItemNtc();
                itemUpdateNtc.UpdateType = ItemNoticeType.ResetCraftpoint;
                itemUpdateNtc.UpdateWalletList.Add(new CDataUpdateWalletPoint()
                {
                    Type = WalletType.ResetCraftSkills,
                    Value = resetCraftSkillWalletPoint.Value,
                    AddPoint = 1,
                    ExtraBonusPoint = 0
                });
                Server.Database.UpdateWalletPoint(client.Character.CharacterId, resetCraftSkillWalletPoint);
                client.Send(itemUpdateNtc);
            }

            if (Server.CraftManager.CanPawnExpUp(leadPawn))
            {
                Server.CraftManager.HandlePawnExpUp(client, leadPawn, craftProgress.Exp, craftProgress.BonusExp);
            }

            if (Server.CraftManager.CanPawnRankUp(leadPawn))
            {
                Server.CraftManager.HandlePawnRankUp(client, leadPawn);
            }

            Server.Database.UpdatePawnBaseInfo(leadPawn);

            // TODO: track and increase CraftCount for NTC 8.35.16

            return craftProductInfoRes;
        }
    }
}
