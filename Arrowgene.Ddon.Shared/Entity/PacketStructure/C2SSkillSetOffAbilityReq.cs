using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillSetOffAbilityReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_SET_OFF_ABILITY_REQ;

        public byte SlotNo { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSkillSetOffAbilityReq>
        {
            public override void Write(IBuffer buffer, C2SSkillSetOffAbilityReq obj)
            {
                WriteByte(buffer, obj.SlotNo);
            }

            public override C2SSkillSetOffAbilityReq Read(IBuffer buffer)
            {
                C2SSkillSetOffAbilityReq obj = new C2SSkillSetOffAbilityReq();
                obj.SlotNo = ReadByte(buffer);
                return obj;
            }
        }
    }
}