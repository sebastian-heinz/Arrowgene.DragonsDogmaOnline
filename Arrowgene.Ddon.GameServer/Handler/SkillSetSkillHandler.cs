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
    public class SkillSetSkillHandler : StructurePacketHandler<GameClient, C2SSkillSetSkillReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillSetSkillHandler));

        private readonly DdonGameServer gameServer;

        public SkillSetSkillHandler(DdonGameServer server) : base(server)
        {
            gameServer = server;
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillSetSkillReq> packet)
        {
            CustomSkill skillSlot = gameServer.SkillManager.SetSkill(Server.Database, client, client.Character, packet.Structure.Job, packet.Structure.SlotNo, packet.Structure.SkillId, packet.Structure.SkillLv);
            client.Send(new S2CSkillSetSkillRes() {
                Job = skillSlot.Job,
                SlotNo = skillSlot.SlotNo,
                SkillId = skillSlot.SkillId,
                SkillLv = skillSlot.SkillLv
            });
        }
    }
}