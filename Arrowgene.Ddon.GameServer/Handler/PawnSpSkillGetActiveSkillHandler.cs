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
    public class PawnSpSkillGetActiveSkillHandler : GameStructurePacketHandler<C2SPawnSpSkillGetActiveSkillReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PawnSpSkillGetActiveSkillHandler));
        
        public PawnSpSkillGetActiveSkillHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPawnSpSkillGetActiveSkillReq> packet)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            S2CPawnSpSkillGetActiveSkillRes res = new S2CPawnSpSkillGetActiveSkillRes();
            res.SpSkillList = pawn.SpSkills.GetValueOrDefault(packet.Structure.JobId, new List<CDataSpSkill>());
            res.ActiveSpSkillSlots = 3; // Value taken from the tutorial picture
            client.Send(res);
        }
    }
}