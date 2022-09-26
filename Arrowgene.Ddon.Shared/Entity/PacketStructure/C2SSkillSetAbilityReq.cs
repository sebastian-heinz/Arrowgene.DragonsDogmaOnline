using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillSetAbilityReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_SET_ABILITY_REQ;

        public JobId Job { get; set; }
        public byte SlotNo { get; set; }
        public uint SkillId { get; set; }
        public byte SkillLv { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSkillSetAbilityReq>
        {
            public override void Write(IBuffer buffer, C2SSkillSetAbilityReq obj)
            {
                WriteByte(buffer, (byte) obj.Job);
                WriteByte(buffer, obj.SlotNo);
                WriteUInt32(buffer, obj.SkillId);
                WriteByte(buffer, obj.SkillLv);
            }

            public override C2SSkillSetAbilityReq Read(IBuffer buffer)
            {
                C2SSkillSetAbilityReq obj = new C2SSkillSetAbilityReq();
                obj.Job = (JobId) ReadByte(buffer);
                obj.SlotNo = ReadByte(buffer);
                obj.SkillId = ReadUInt32(buffer);
                obj.SkillLv = ReadByte(buffer);
                return obj;
            }
        }
    }
}