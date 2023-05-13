using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetLearnedSkillListHandler : StructurePacketHandler<GameClient, C2SSkillGetLearnedSkillListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetLearnedSkillListHandler));

        public SkillGetLearnedSkillListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillGetLearnedSkillListReq> packet)
        {
            client.Send(new S2CSkillGetLearnedSkillListRes()
            {
                SetAcquierementParam = client.Character.LearnedCustomSkills
            });
        }
    }
}