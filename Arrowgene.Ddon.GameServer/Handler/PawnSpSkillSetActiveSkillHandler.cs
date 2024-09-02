using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
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
            
            List<CDataSpSkill> spSkills;
            if (pawn.SpSkills.ContainsKey(packet.Structure.JobId))
            {
                spSkills = pawn.SpSkills[packet.Structure.JobId];
            }
            else
            {
                spSkills = new List<CDataSpSkill>();
                pawn.SpSkills.Add(packet.Structure.JobId, spSkills);
            }

            if(packet.Structure.ToActiveSpSkill.SpSkillId == 0 && spSkills.Count < 3)
            {
                // Add to an empty slot
                spSkills.Add(packet.Structure.FromStockSpSkill);
                Server.Database.InsertSpSkill(pawn.PawnId, packet.Structure.JobId, packet.Structure.FromStockSpSkill);
            }
            else if(spSkills.IndexOf(packet.Structure.ToActiveSpSkill) is int index && index != -1)
            {
                // Replace skill in slot
                spSkills[index] = packet.Structure.FromStockSpSkill;
                Server.Database.DeleteSpSkill(pawn.PawnId, packet.Structure.JobId, packet.Structure.ToActiveSpSkill.SpSkillId);
                Server.Database.InsertSpSkill(pawn.PawnId, packet.Structure.JobId, packet.Structure.FromStockSpSkill);
            }
            else
            {
                res.Error = (uint) ErrorCode.ERROR_CODE_FAIL;
            }

            client.Send(res);
        }
    }
}