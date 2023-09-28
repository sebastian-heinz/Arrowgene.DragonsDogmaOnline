using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
    
namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillLearnPawnAbilityReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_LEARN_PAWN_ABILITY_REQ;

        public uint PawnId { get; set; }
        public uint AbilityId { get; set; }
        public byte AbilityLv { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSkillLearnPawnAbilityReq>
        {
            public override void Write(IBuffer buffer, C2SSkillLearnPawnAbilityReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteUInt32(buffer, obj.AbilityId);
                WriteByte(buffer, obj.AbilityLv);
            }

            public override C2SSkillLearnPawnAbilityReq Read(IBuffer buffer)
            {
                C2SSkillLearnPawnAbilityReq obj = new C2SSkillLearnPawnAbilityReq();
                obj.PawnId = ReadUInt32(buffer);
                obj.AbilityId = ReadUInt32(buffer);
                obj.AbilityLv = ReadByte(buffer);
                return obj;
            }
        }

    }
}