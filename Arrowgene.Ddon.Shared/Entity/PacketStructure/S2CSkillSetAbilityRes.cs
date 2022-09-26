using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillSetAbilityRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_SET_ABILITY_RES;

        public byte SlotNo { get; set; }
        public uint AbilityId { get; set; }
        public byte AbilityLv { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillSetAbilityRes>
        {
            public override void Write(IBuffer buffer, S2CSkillSetAbilityRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, obj.SlotNo);
                WriteUInt32(buffer, obj.AbilityId);
                WriteByte(buffer, obj.AbilityLv);
            }

            public override S2CSkillSetAbilityRes Read(IBuffer buffer)
            {
                S2CSkillSetAbilityRes obj = new S2CSkillSetAbilityRes();
                ReadServerResponse(buffer, obj);
                obj.SlotNo = ReadByte(buffer);
                obj.AbilityId = ReadUInt32(buffer);
                obj.AbilityLv = ReadByte(buffer);
                return obj;
            }
        }
    }
}