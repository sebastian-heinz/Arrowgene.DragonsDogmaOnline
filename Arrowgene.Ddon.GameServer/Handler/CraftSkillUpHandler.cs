#nullable enable
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CraftSkillUpHandler : GameStructurePacketHandler<C2SCraftSkillUpReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CraftSkillUpHandler));

        public CraftSkillUpHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SCraftSkillUpReq> packet)
        {
            S2CCraftSkillUpRes craftSkillUpRes = new S2CCraftSkillUpRes
            {
                PawnID = packet.Structure.PawnID,
                SkillType = packet.Structure.SkillType,
                SkillLevel = packet.Structure.SkillLevel
            };

            Pawn pawn = client.Character.Pawns.Find(p => p.PawnId == packet.Structure.PawnID);

            CDataPawnCraftSkill pawnCraftSkill = pawn.CraftData.PawnCraftSkillList.Find(skill => skill.Type == packet.Structure.SkillType);
            uint previousCraftSkillLevel = pawnCraftSkill.Level;
            pawnCraftSkill.Level = packet.Structure.SkillLevel;

            craftSkillUpRes.UseCraftPoint = pawnCraftSkill.Level - previousCraftSkillLevel;

            pawn.CraftData.CraftPoint -= craftSkillUpRes.UseCraftPoint;
            craftSkillUpRes.RemainCraftPoint = pawn.CraftData.CraftPoint;

            Server.Database.UpdatePawnBaseInfo(pawn);

            client.Send(craftSkillUpRes);
        }
    }
}
