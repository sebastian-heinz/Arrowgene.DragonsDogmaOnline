using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillSetOffAbilityRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_SET_OFF_ABILITY_RES;

        public byte SlotNo;

        public class Serializer : PacketEntitySerializer<S2CSkillSetOffAbilityRes>
        {
            public override void Write(IBuffer buffer, S2CSkillSetOffAbilityRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, obj.SlotNo);
            }

            public override S2CSkillSetOffAbilityRes Read(IBuffer buffer)
            {
                S2CSkillSetOffAbilityRes obj = new S2CSkillSetOffAbilityRes();
                ReadServerResponse(buffer, obj);
                obj.SlotNo = ReadByte(buffer);
                return obj;
            }
        }
    }
}