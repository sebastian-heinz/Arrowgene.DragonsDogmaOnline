using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillCustomSkillSetNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_SKILL_CUSTOM_SKILL_SET_NTC;

        public uint CharacterId { get; set; }
        public CDataContextAcquirementData ContextAcquirementData { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CSkillCustomSkillSetNtc>
        {
            public override void Write(IBuffer buffer, S2CSkillCustomSkillSetNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteEntity<CDataContextAcquirementData>(buffer, obj.ContextAcquirementData);
            }

            public override S2CSkillCustomSkillSetNtc Read(IBuffer buffer)
            {
                S2CSkillCustomSkillSetNtc obj = new S2CSkillCustomSkillSetNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.ContextAcquirementData = ReadEntity<CDataContextAcquirementData>(buffer);
                return obj;
            }
        }
    }
}
