using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetLearnedNormalSkillListHandler : GameRequestPacketHandler<C2SSkillGetLearnedNormalSkillListReq, S2CSkillGetLearnedNormalSkillListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetLearnedNormalSkillListHandler));

        public SkillGetLearnedNormalSkillListHandler(DdonGameServer server) : base(server)
        {
        }

        // Learned Core Skills
        public override S2CSkillGetLearnedNormalSkillListRes Handle(GameClient client, C2SSkillGetLearnedNormalSkillListReq request)
        {
            return new S2CSkillGetLearnedNormalSkillListRes()
            {
                NormalSkillParamList = client.Character.LearnedNormalSkills
            };
        }
    }
}
