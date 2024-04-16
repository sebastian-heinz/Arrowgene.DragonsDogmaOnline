using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetLearnedNormalSkillListHandler : GameStructurePacketHandler<C2SSkillGetLearnedNormalSkillListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetLearnedNormalSkillListHandler));

        public SkillGetLearnedNormalSkillListHandler(DdonGameServer server) : base(server)
        {
        }

        // Learned Core Skills
        public override void Handle(GameClient client, StructurePacket<C2SSkillGetLearnedNormalSkillListReq> packet)
        {
            var Result = new S2CSkillGetLearnedNormalSkillListRes()
            {
                NormalSkillParamList = client.Character.LearnedNormalSkills
            };

            client.Send(Result);
        }

    }
}
