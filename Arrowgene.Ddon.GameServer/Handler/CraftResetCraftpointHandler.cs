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

            CDataWalletPoint resetCraftSkillWalletPoint = client.Character.WalletPointList.Find(l => l.Type == WalletType.ResetCraftSkills);
            resetCraftSkillWalletPoint.Value--;
            S2CItemUpdateCharacterItemNtc itemUpdateNtc = new S2CItemUpdateCharacterItemNtc();
            itemUpdateNtc.UpdateType = ItemNoticeType.ResetCraftpoint;
            itemUpdateNtc.UpdateWalletList.Add(new CDataUpdateWalletPoint()
            {
                Type = WalletType.ResetCraftSkills,
                Value = resetCraftSkillWalletPoint.Value,
                AddPoint = -1,
                ExtraBonusPoint = 0
            });
            Server.Database.UpdateWalletPoint(client.Character.CharacterId, resetCraftSkillWalletPoint);

            client.Send(itemUpdateNtc);
            client.Send(craftResetCraftpointRes);
        }
    }
}
