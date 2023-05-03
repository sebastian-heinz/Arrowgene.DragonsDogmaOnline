using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillGetPawnAbilityCostReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_GET_PAWN_ABILITY_COST_REQ;

        public uint PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSkillGetPawnAbilityCostReq>
        {
            public override void Write(IBuffer buffer, C2SSkillGetPawnAbilityCostReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }

            public override C2SSkillGetPawnAbilityCostReq Read(IBuffer buffer)
            {
                C2SSkillGetPawnAbilityCostReq obj = new C2SSkillGetPawnAbilityCostReq();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}