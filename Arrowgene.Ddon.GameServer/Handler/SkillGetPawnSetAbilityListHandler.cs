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
    public class SkillGetPawnSetAbilityListHandler : GameStructurePacketHandler<C2SSkillGetPawnSetAbilityListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetPawnSetAbilityListHandler));
        
        public SkillGetPawnSetAbilityListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillGetPawnSetAbilityListReq> packet)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            // TODO: Check if its necessary to filter so only the current job skills are sent
            S2CSkillGetPawnSetAbilityListRes res = new S2CSkillGetPawnSetAbilityListRes();
            res.PawnId = pawn.PawnId;
            res.SetAcquierementParamList = pawn.EquippedAbilitiesDictionary[pawn.Job]
                .Select((x, index) => x?.AsCDataSetAcquirementParam((byte)(index+1)))
                .Where(x => x != null)
                .ToList();
            client.Send(res);
        }
    }
}