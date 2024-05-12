using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetPawnAbilityCostHandler : GameStructurePacketHandler<C2SSkillGetPawnAbilityCostReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetPawnAbilityCostHandler));

        private readonly CharacterManager _CharacterManager;

        public SkillGetPawnAbilityCostHandler(DdonGameServer server) : base(server)
        {
            _CharacterManager = server.CharacterManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillGetPawnAbilityCostReq> packet)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();

            client.Send(new S2CSkillGetPawnAbilityCostRes()
            {
                PawnId = packet.Structure.PawnId,
                CostMax = _CharacterManager.GetMaxAugmentAllocation(pawn)
            });
        }
    }
}
