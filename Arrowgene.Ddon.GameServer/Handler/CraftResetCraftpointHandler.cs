#nullable enable
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftResetCraftpointHandler : GameStructurePacketHandler<C2SCraftResetCraftpointReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftResetCraftpointHandler));

        public CraftResetCraftpointHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCraftResetCraftpointReq> packet)
        {
            S2CCraftResetCraftpointRes craftResetCraftpointRes = new S2CCraftResetCraftpointRes
            {
                PawnID = packet.Structure.PawnID
            };

            Pawn pawn = client.Character.Pawns.Find(p => p.PawnId == packet.Structure.PawnID);
            pawn.CraftData.CraftPoint = pawn.CraftData.CraftRank - 1;
            foreach (CDataPawnCraftSkill skill in pawn.CraftData.PawnCraftSkillList)
            {
                skill.Level = 0;
            }

            craftResetCraftpointRes.CraftPoint = pawn.CraftData.CraftPoint;
            craftResetCraftpointRes.CraftSkillList = pawn.CraftData.PawnCraftSkillList;
            Server.Database.UpdatePawnBaseInfo(pawn);

            S2CItemUpdateCharacterItemNtc itemUpdateNtc = new S2CItemUpdateCharacterItemNtc();
            itemUpdateNtc.UpdateType = ItemNoticeType.ResetCraftpoint;
            itemUpdateNtc.UpdateWalletList.Add(Server.WalletManager.RemoveFromWallet(
                client.Character,
                WalletType.ResetCraftSkills,
                1
            ));
            client.Send(itemUpdateNtc);
            client.Send(craftResetCraftpointRes);
        }
    }
}
