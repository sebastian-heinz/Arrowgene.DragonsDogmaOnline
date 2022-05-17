using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillSetSkillRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_SET_SKILL_RES;

        public JobId Job { get; set; }
        public byte SlotNo { get; set; }
        public uint SkillId { get; set; }
        public byte SkillLv { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillSetSkillRes>
        {
            public override void Write(IBuffer buffer, S2CSkillSetSkillRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, (byte) obj.Job);
                WriteByte(buffer, obj.SlotNo);
                WriteUInt32(buffer, obj.SkillId);
                WriteByte(buffer, obj.SkillLv);
            }

            public override S2CSkillSetSkillRes Read(IBuffer buffer)
            {
                S2CSkillSetSkillRes obj = new S2CSkillSetSkillRes();
                ReadServerResponse(buffer, obj);
                obj.Job = (JobId) ReadByte(buffer);
                obj.SlotNo = ReadByte(buffer);
                obj.SkillId = ReadUInt32(buffer);
                obj.SkillLv = ReadByte(buffer);
                return obj;
            }
        }
    }
}