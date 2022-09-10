using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillCustomSkillSetNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_SKILL_CUSTOM_SKILL_SET_NTC;

        public uint CharacterId { get; set; }
        public byte SlotNo { get; set; }
        public uint AcquirementNo { get; set; }
        public byte AcquirementLv { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillCustomSkillSetNtc>
        {
            public override void Write(IBuffer buffer, S2CSkillCustomSkillSetNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteByte(buffer, obj.SlotNo);
                WriteUInt32(buffer, obj.AcquirementNo);
                WriteByte(buffer, obj.AcquirementLv);
            }

            public override S2CSkillCustomSkillSetNtc Read(IBuffer buffer)
            {
                S2CSkillCustomSkillSetNtc obj = new S2CSkillCustomSkillSetNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.SlotNo = ReadByte(buffer);
                obj.AcquirementNo = ReadUInt32(buffer);
                obj.AcquirementLv = ReadByte(buffer);
                return obj;
            }
        }
    }
}