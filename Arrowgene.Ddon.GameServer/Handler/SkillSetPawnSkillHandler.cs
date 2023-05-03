using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillSetPawnSkillHandler : GameStructurePacketHandler<C2SSkillSetPawnSkillReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillSetPawnSkillHandler));
        
        private readonly DdonGameServer gameServer;

        public SkillSetPawnSkillHandler(DdonGameServer server) : base(server)
        {
            gameServer = server;
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillSetPawnSkillReq> packet)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            CustomSkill skillSlot = gameServer.JobManager.SetSkill(Server.Database, client, pawn, packet.Structure.Job, packet.Structure.SlotNo, packet.Structure.SkillId, packet.Structure.SkillLv);
            client.Send(new S2CSkillSetPawnSkillRes() {
                PawnId = pawn.PawnId,
                Job = skillSlot.Job,
                SlotNo = skillSlot.SlotNo,
                SkillId = skillSlot.SkillId,
                SkillLv = skillSlot.SkillLv
            });
        }
    }
}