using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillGetPawnAbilityCostRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_GET_PAWN_ABILITY_COST_RES;

        public uint PawnId { get; set; }
        public uint CostMax { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillGetPawnAbilityCostRes>
        {
            public override void Write(IBuffer buffer, S2CSkillGetPawnAbilityCostRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteUInt32(buffer, obj.CostMax);
            }

            public override S2CSkillGetPawnAbilityCostRes Read(IBuffer buffer)
            {
                S2CSkillGetPawnAbilityCostRes obj = new S2CSkillGetPawnAbilityCostRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.CostMax = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}