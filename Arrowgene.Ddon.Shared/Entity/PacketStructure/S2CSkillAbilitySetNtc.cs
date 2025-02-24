using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillAbilitySetNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_SKILL_ABILITY_SET_NTC;

        public uint CharacterId { get; set; }
        public CDataContextAcquirementData ContextAcquirementData { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CSkillAbilitySetNtc>
        {
            public override void Write(IBuffer buffer, S2CSkillAbilitySetNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteEntity<CDataContextAcquirementData>(buffer, obj.ContextAcquirementData);
            }

            public override S2CSkillAbilitySetNtc Read(IBuffer buffer)
            {
                S2CSkillAbilitySetNtc obj = new S2CSkillAbilitySetNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.ContextAcquirementData = ReadEntity<CDataContextAcquirementData>(buffer);
                return obj;
            }
        }
    }
}
