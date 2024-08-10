using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillPawnAbilitySetNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_SKILL_PAWN_ABILITY_SET_NTC;

        public S2CSkillPawnAbilitySetNtc()
        {
            ContextAcquirementData = new CDataContextAcquirementData();
        }

        public uint PawnId { get; set; }
        public CDataContextAcquirementData ContextAcquirementData { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillPawnAbilitySetNtc>
        {
            public override void Write(IBuffer buffer, S2CSkillPawnAbilitySetNtc obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteEntity<CDataContextAcquirementData>(buffer, obj.ContextAcquirementData);
            }

            public override S2CSkillPawnAbilitySetNtc Read(IBuffer buffer)
            {
                S2CSkillPawnAbilitySetNtc obj = new S2CSkillPawnAbilitySetNtc();
                obj.PawnId = ReadUInt32(buffer);
                obj.ContextAcquirementData = ReadEntity<CDataContextAcquirementData>(buffer);
                return obj;
            }
        }
    }
}
