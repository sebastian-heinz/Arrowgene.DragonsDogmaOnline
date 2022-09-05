using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillSetOffSkillRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_SET_OFF_SKILL_RES;

        public JobId Job { get; set; }
        public byte SlotNo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillSetOffSkillRes>
        {
            public override void Write(IBuffer buffer, S2CSkillSetOffSkillRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, (byte) obj.Job);
                WriteByte(buffer, obj.SlotNo);
            }

            public override S2CSkillSetOffSkillRes Read(IBuffer buffer)
            {
                S2CSkillSetOffSkillRes obj = new S2CSkillSetOffSkillRes();
                ReadServerResponse(buffer, obj);
                obj.Job = (JobId) ReadByte(buffer);
                obj.SlotNo = ReadByte(buffer);
                return obj;
            }
        }
    }
}