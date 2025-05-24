#nullable enable
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftResetCraftpointHandler : GameRequestPacketHandler<C2SCraftResetCraftpointReq, S2CCraftResetCraftpointRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftResetCraftpointHandler));

        public CraftResetCraftpointHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCraftResetCraftpointRes Handle(GameClient client, C2SCraftResetCraftpointReq request)
        {
            S2CCraftResetCraftpointRes craftResetCraftpointRes = new S2CCraftResetCraftpointRes
            {
                PawnID = request.PawnID
            };

            Pawn pawn = client.Character.Pawns.Find(p => p.PawnId == request.PawnID)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PAWN_NOT_FOUNDED,
                $"Couldn't find pawn ID {request.PawnID}.");
            pawn.CraftData.CraftPoint = pawn.CraftData.CraftRank - 1;
            foreach (CDataPawnCraftSkill skill in pawn.CraftData.PawnCraftSkillList)
            {
                skill.Level = 0;
            }

            craftResetCraftpointRes.CraftPoint = pawn.CraftData.CraftPoint;
            craftResetCraftpointRes.CraftSkillList = pawn.CraftData.PawnCraftSkillList;
            Server.Database.UpdatePawnBaseInfo(pawn);

            var updateWalletPoint = Server.WalletManager.RemoveFromWallet(
                client.Character,
                WalletType.ResetCraftSkills,
                1
            ) ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_CRAFT_SKILL_RESET_ZERO_POINT);

            S2CItemUpdateCharacterItemNtc itemUpdateNtc = new S2CItemUpdateCharacterItemNtc();
            itemUpdateNtc.UpdateType = ItemNoticeType.ResetCraftpoint;
            itemUpdateNtc.UpdateWalletList.Add(updateWalletPoint);
            client.Send(itemUpdateNtc);
            return craftResetCraftpointRes;
        }
    }
}
