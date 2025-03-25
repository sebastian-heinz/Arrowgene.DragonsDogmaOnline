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
    public class PawnSpSkillSetActiveSkillHandler : GameRequestPacketHandler<C2SPawnSpSkillSetActiveSkillReq, S2CPawnSpSkillSetActiveSkillRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnSpSkillSetActiveSkillHandler));
        
        public PawnSpSkillSetActiveSkillHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnSpSkillSetActiveSkillRes Handle(GameClient client, C2SPawnSpSkillSetActiveSkillReq request)
        {
            S2CPawnSpSkillSetActiveSkillRes res = new S2CPawnSpSkillSetActiveSkillRes();
            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);

            List<CDataSpSkill> spSkills;
            if (pawn.SpSkills.ContainsKey(request.JobId))
            {
                spSkills = pawn.SpSkills[request.JobId];
            }
            else
            {
                spSkills = new List<CDataSpSkill>();
                pawn.SpSkills.Add(request.JobId, spSkills);
            }

            if(request.ToActiveSpSkill.SpSkillId == 0 && spSkills.Count < 3)
            {
                // Add to an empty slot
                spSkills.Add(request.FromStockSpSkill);
                Server.Database.InsertSpSkill(pawn.PawnId, request.JobId, request.FromStockSpSkill);
            }
            else if(spSkills.IndexOf(request.ToActiveSpSkill) is int index && index != -1)
            {
                // Replace skill in slot
                spSkills[index] = request.FromStockSpSkill;
                Server.Database.DeleteSpSkill(pawn.PawnId, request.JobId, request.ToActiveSpSkill.SpSkillId);
                Server.Database.InsertSpSkill(pawn.PawnId, request.JobId, request.FromStockSpSkill);
            }
            else
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_SKILL_INVALID_SLOT_NO);
            }

            return res;
        }

    }
}
