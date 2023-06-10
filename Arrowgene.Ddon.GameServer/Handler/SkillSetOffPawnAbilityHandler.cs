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
    public class SkillSetOffPawnAbilityHandler : GameStructurePacketHandler<C2SSkillSetOffPawnAbilityReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillSetOffPawnAbilityHandler));
        
        private readonly JobManager jobManager;

        public SkillSetOffPawnAbilityHandler(DdonGameServer server) : base(server)
        {
            jobManager = server.JobManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillSetOffPawnAbilityReq> packet)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            jobManager.RemoveAbility(Server.Database, pawn, packet.Structure.SlotNo);

            client.Send(new S2CSkillSetOffPawnAbilityRes()
            {
                PawnId = packet.Structure.PawnId,
                SlotNo = packet.Structure.SlotNo
            });
        }
    }
}