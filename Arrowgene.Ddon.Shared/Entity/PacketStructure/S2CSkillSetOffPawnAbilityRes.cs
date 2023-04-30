using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillSetOffPawnAbilityRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_SET_OFF_PAWN_ABILITY_RES;

        public uint PawnId { get; set; }
        public byte SlotNo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillSetOffPawnAbilityRes>
        {
            public override void Write(IBuffer buffer, S2CSkillSetOffPawnAbilityRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, obj.SlotNo);
            }

            public override S2CSkillSetOffPawnAbilityRes Read(IBuffer buffer)
            {
                S2CSkillSetOffPawnAbilityRes obj = new S2CSkillSetOffPawnAbilityRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.SlotNo = ReadByte(buffer);
                return obj;
            }
        }
    }
}