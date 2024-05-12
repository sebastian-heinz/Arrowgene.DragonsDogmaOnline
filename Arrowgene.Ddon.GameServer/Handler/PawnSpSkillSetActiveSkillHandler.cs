using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnSpSkillSetActiveSkillHandler : GameStructurePacketHandler<C2SPawnSpSkillSetActiveSkillReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnSpSkillSetActiveSkillHandler));
        
        public PawnSpSkillSetActiveSkillHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnSpSkillSetActiveSkillReq> packet)
        {
            S2CPawnSpSkillSetActiveSkillRes res = new S2CPawnSpSkillSetActiveSkillRes();
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            if(packet.Structure.ToActiveSpSkill.SpSkillId == 0 && pawn.SpSkillList.Count < 3)
            {
                // Add to an empty slot
                pawn.SpSkillList.Add(packet.Structure.FromStockSpSkill);
                Server.Database.InsertSpSkill(pawn.PawnId, packet.Structure.FromStockSpSkill);
            }
            else if(pawn.SpSkillList.IndexOf(packet.Structure.ToActiveSpSkill) is int index && index != -1)
            {
                // Replace skill in slot
                pawn.SpSkillList[index] = packet.Structure.FromStockSpSkill;
                Server.Database.DeleteSpSkill(pawn.PawnId, packet.Structure.ToActiveSpSkill.SpSkillId);
                Server.Database.InsertSpSkill(pawn.PawnId, packet.Structure.FromStockSpSkill);
            }
            else
            {
                res.Error = (uint) ErrorCode.ERROR_CODE_FAIL;
            }

            client.Send(res);
        }
    }
}