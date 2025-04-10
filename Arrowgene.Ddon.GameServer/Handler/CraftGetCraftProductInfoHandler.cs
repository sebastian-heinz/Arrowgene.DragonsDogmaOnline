using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftGetCraftProductInfoHandler : GameRequestPacketQueueHandler<C2SCraftGetCraftProductInfoReq, S2CCraftGetCraftProductInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftGetCraftProductInfoHandler));

        public CraftGetCraftProductInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SCraftGetCraftProductInfoReq request)
        {
            PacketQueue queue = new();
            S2CCraftGetCraftProductInfoRes craftProductInfoRes = new S2CCraftGetCraftProductInfoRes();

            Server.Database.ExecuteInTransaction(connection =>
            {
                CraftProgress craftProgress = Server.Database.SelectPawnCraftProgress(client.Character.CharacterId, request.CraftMainPawnID, connection);
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
                    client.Enqueue(CraftManager.HandlePawnExpUpNtc(client, leadPawn, 0, 0), queue);
                    // TODO: This is not accurate to the original game but currently there is no other way to gain crafting reset points.
                    client.Enqueue(Server.WalletManager.AddToWalletNtc(client, client.Character, WalletType.ResetCraftSkills, 1, 0, ItemNoticeType.ResetCraftpoint, connection), queue);
                    Server.Database.UpdatePawnBaseInfo(leadPawn, connection);
                    queue.AddRange(Server.AchievementManager.HandlePawnCraftingExam(client, leadPawn, connection));
                }
                else
                {
                    if (CraftManager.CanPawnExpUp(leadPawn))
                    {
                        double BonusExpMultiplier = Server.GpCourseManager.PawnCraftBonus();
                        client.Enqueue(CraftManager.HandlePawnExpUpNtc(client, leadPawn, craftProgress.Exp, BonusExpMultiplier), queue);
                        if (CraftManager.CanPawnRankUp(leadPawn))
                        {
                            client.Enqueue(CraftManager.HandlePawnRankUpNtc(client, leadPawn), queue);
                            queue.AddRange(Server.AchievementManager.HandlePawnCrafting(client, leadPawn, connection));
                        }

                        Server.Database.UpdatePawnBaseInfo(leadPawn, connection);
                    }
                    else
                    {
                        // Mandatory to send otherwise the UI gets stuck.
                        client.Enqueue(CraftManager.HandlePawnExpUpNtc(client, leadPawn, 0, 0), queue);
                    }
                }
                craftProductInfoRes.CraftProductInfo = craftProductInfo;
            });

            // TODO: track and increase CraftCount for NTC 8.35.16
            client.Enqueue(craftProductInfoRes, queue);

            return queue;
        }
    }
}
