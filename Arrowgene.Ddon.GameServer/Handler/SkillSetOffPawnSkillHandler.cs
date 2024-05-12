using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillSetOffPawnSkillHandler : GameStructurePacketHandler<C2SSkillSetOffPawnSkillReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillSetOffPawnSkillHandler));
        
        private readonly JobManager jobManager;

        public SkillSetOffPawnSkillHandler(DdonGameServer server) : base(server)
        {
            jobManager = server.JobManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillSetOffPawnSkillReq> packet)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            jobManager.RemoveSkill(Server.Database, pawn, packet.Structure.Job, packet.Structure.SlotNo);

            client.Send(new S2CSkillSetOffPawnSkillRes() {
                PawnId = packet.Structure.PawnId,
                Job = packet.Structure.Job,
                SlotNo = packet.Structure.SlotNo
            });
        }
    }
}