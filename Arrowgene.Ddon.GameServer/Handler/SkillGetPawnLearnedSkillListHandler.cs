using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetPawnLearnedSkillListHandler : GameStructurePacketHandler<C2SSkillGetPawnLearnedSkillListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetPawnLearnedSkillListHandler));
        
        public SkillGetPawnLearnedSkillListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillGetPawnLearnedSkillListReq> packet)
        {
            // TODO: Move this to DB
            client.Send(new S2CSkillGetPawnLearnedSkillListRes() {
                LearnedAcquierementParamList = SkillGetAcquirableSkillListHandler.AllSkills
                    //.Where(skill => !SkillGetLearnedSkillListHandler.IsSkillEX(skill.SkillNo))
                    .Select(skill => new CDataLearnedSetAcquirementParam() {
                        Job = skill.Job,
                        Type = skill.Type,
                        AcquirementNo = skill.SkillNo,
                        AcquirementLv = (byte) (SkillGetLearnedSkillListHandler.IsSkillEX(skill.SkillNo) ? 1 : 10) // EX skills must be Lv 1 to work, otherwise use Lv 10 (Max level)
                    }).ToList()
            });
        }
    }
}