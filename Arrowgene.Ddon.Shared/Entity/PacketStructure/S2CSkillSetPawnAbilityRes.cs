using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillSetPawnAbilityRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_SET_PAWN_ABILITY_RES;

        public uint PawnId { get; set; }
        public byte SlotNo { get; set; }
        public uint AbilityId { get; set; }
        public byte AbilityLv { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillSetPawnAbilityRes>
        {
            public override void Write(IBuffer buffer, S2CSkillSetPawnAbilityRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, obj.SlotNo);
                WriteUInt32(buffer, obj.AbilityId);
                WriteByte(buffer, obj.AbilityLv);
            }

            public override S2CSkillSetPawnAbilityRes Read(IBuffer buffer)
            {
                S2CSkillSetPawnAbilityRes obj = new S2CSkillSetPawnAbilityRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.SlotNo = ReadByte(buffer);
                obj.AbilityId = ReadUInt32(buffer);
                obj.AbilityLv = ReadByte(buffer);
                return obj;
            }
        }
    }
}