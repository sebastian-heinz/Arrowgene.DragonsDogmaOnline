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
            // TODO: check if course bonus provides exp bonus for crafting
            // TODO: calculate bonus EXP now that remaining time is 0
            // TODO: Decide whether bonus exp should be calculated when craft is started vs. received
            bool expBonus = false;
            uint bonusExp = 0;
            if (expBonus)
            {
                bonusExp = 100;
            }

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
            Server.CraftManager.HandlePawnExpUp(client, leadPawn, craftProgress.Exp, craftProgress.BonusExp);
            Server.CraftManager.HandlePawnRankUp(client, leadPawn);
            Server.Database.UpdatePawnBaseInfo(leadPawn);

            return craftProductInfoRes;
        }
    }
}
