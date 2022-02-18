using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetLearnedNormalSkillListHandler : StructurePacketHandler<GameClient, C2SSkillGetLearnedNormalSkillListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetLearnedNormalSkillListHandler));

        public SkillGetLearnedNormalSkillListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillGetLearnedNormalSkillListReq> packet)
        {
            client.Send(new S2CSkillGetLearnedNormalSkillListRes());
        }
    }
}