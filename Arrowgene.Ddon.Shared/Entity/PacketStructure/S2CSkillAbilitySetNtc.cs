using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillAbilitySetNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_SKILL_19_46_16_NTC;

        public uint CharacterId { get; set; }
        public CDataContextAcquirementData ContextAcquirementData { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillAbilitySetNtc>
        {
            public override void Write(IBuffer buffer, S2CSkillAbilitySetNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteByte(buffer, obj.ContextAcquirementData.SlotNo);
                WriteUInt32(buffer, obj.ContextAcquirementData.AcquirementNo);
                WriteByte(buffer, obj.ContextAcquirementData.AcquirementLv);
            }

            public override S2CSkillAbilitySetNtc Read(IBuffer buffer)
            {
                S2CSkillAbilitySetNtc obj = new S2CSkillAbilitySetNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.ContextAcquirementData.SlotNo = ReadByte(buffer);
                obj.ContextAcquirementData.AcquirementNo = ReadUInt32(buffer);
                obj.ContextAcquirementData.AcquirementLv = ReadByte(buffer);
                return obj;
            }
        }
    }
}