using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillGetAbilityCostRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_GET_ABILITY_COST_RES;

        public S2CSkillGetAbilityCostRes()
        {
            CostMax=0;
        }

        public uint CostMax { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillGetAbilityCostRes>
        {
            public override void Write(IBuffer buffer, S2CSkillGetAbilityCostRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.CostMax);
            }

            public override S2CSkillGetAbilityCostRes Read(IBuffer buffer)
            {
                S2CSkillGetAbilityCostRes obj = new S2CSkillGetAbilityCostRes();
                ReadServerResponse(buffer, obj);
                obj.CostMax = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
