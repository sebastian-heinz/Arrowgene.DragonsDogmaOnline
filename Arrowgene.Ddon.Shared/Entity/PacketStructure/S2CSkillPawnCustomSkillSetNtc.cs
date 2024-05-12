using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillPawnCustomSkillSetNtc : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_19_44_16_NTC; // Not 100% sure, only testing will tell

        public S2CSkillPawnCustomSkillSetNtc()
        {
            ContextAcquirementData = new CDataContextAcquirementData();
        }

        public uint PawnId { get; set; }
        public CDataContextAcquirementData ContextAcquirementData { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillPawnCustomSkillSetNtc>
        {
            public override void Write(IBuffer buffer, S2CSkillPawnCustomSkillSetNtc obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteEntity<CDataContextAcquirementData>(buffer, obj.ContextAcquirementData);
            }

            public override S2CSkillPawnCustomSkillSetNtc Read(IBuffer buffer)
            {
                S2CSkillPawnCustomSkillSetNtc obj = new S2CSkillPawnCustomSkillSetNtc();
                obj.PawnId = ReadUInt32(buffer);
                obj.ContextAcquirementData = ReadEntity<CDataContextAcquirementData>(buffer);
                return obj;
            }
        }
    }
}