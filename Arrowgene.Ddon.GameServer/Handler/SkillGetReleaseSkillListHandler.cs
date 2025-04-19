using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetReleaseSkillListHandler : GameRequestPacketHandler<C2SSkillGetReleaseSkillListReq, S2CSkillGetReleaseSkillListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetReleaseSkillListHandler));

        public SkillGetReleaseSkillListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSkillGetReleaseSkillListRes Handle(GameClient client, C2SSkillGetReleaseSkillListReq request)
        {
            List<CDataReleaseAcquirementParam> releaseParamList = new();
            foreach (var jobId in ((JobId[])JobId.GetValues(typeof(JobId))))
            {
                var matches = Server.JobMasterManager.GetReleasedElements(client, jobId)
                    .Where(x => x.ReleaseType == ReleaseType.CustomSkill)
                    .Select(x => new CDataReleaseAcquirementParam()
                    {
                        AcquirementLv = x.ReleaseLv,
                        AcquirementNo = x.ReleaseId,
                        JobId = jobId,
                        AcquirementParamID = x.ReleaseId,
                        Type = (byte)x.ReleaseType
                    }).ToList();
                releaseParamList.AddRange(matches);
            }

            return new()
            {
                ReleaseAcquirementParamList = releaseParamList
            };
        }
    }
}
