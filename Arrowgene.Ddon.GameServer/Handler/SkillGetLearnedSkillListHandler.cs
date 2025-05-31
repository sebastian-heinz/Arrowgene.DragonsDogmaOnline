using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetLearnedSkillListHandler : GameRequestPacketHandler<C2SSkillGetLearnedSkillListReq, S2CSkillGetLearnedSkillListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetLearnedSkillListHandler));

        public SkillGetLearnedSkillListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSkillGetLearnedSkillListRes Handle(GameClient client, C2SSkillGetLearnedSkillListReq request)
        {
            var skills = client.GameMode == GameMode.Normal ?
                client.Character.LearnedCustomSkills
                    .Where(x => !SkillData.IsS3HoSkill(x.Job, x.SkillId, x.SkillLv))
                    .Select(x => x.AsCDataLearnedSetAcquirementParam())
                    .ToList()
                : client.Character.LearnedCustomSkills
                    .Select(x => x.AsCDataLearnedSetAcquirementParam())
                    .ToList();

            return new S2CSkillGetLearnedSkillListRes()
            {
                SetAcquirementParam = skills
            };
        }
    }
}
