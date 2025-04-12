using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PawnSpSkillGetActiveSkillHandler : GameRequestPacketHandler<C2SPawnSpSkillGetActiveSkillReq, S2CPawnSpSkillGetActiveSkillRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnSpSkillGetActiveSkillHandler));
        
        public PawnSpSkillGetActiveSkillHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CPawnSpSkillGetActiveSkillRes Handle(GameClient client, C2SPawnSpSkillGetActiveSkillReq request)
        {
            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);
            S2CPawnSpSkillGetActiveSkillRes res = new S2CPawnSpSkillGetActiveSkillRes
            {
                SpSkillList = pawn.SpSkills.GetValueOrDefault(request.JobId, new List<CDataSpSkill>()),
                ActiveSpSkillSlots = 3 // Value taken from the tutorial picture
            };
            return res;
        }
    }
}
