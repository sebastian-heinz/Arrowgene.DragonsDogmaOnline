using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetAcquirableSkillListHandler : GameRequestPacketHandler<C2SSkillGetAcquirableSkillListReq, S2CSkillGetAcquirableSkillListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetCurrentSetSkillListHandler));

        public SkillGetAcquirableSkillListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSkillGetAcquirableSkillListRes Handle(GameClient client, C2SSkillGetAcquirableSkillListReq request)
        {
            return new S2CSkillGetAcquirableSkillListRes()
            {
                SkillParamList = SkillData.AllSkills.Where(x => x.Job == request.Job).ToList()
            };
        }
    }
}
