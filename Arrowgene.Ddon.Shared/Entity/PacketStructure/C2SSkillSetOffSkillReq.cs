using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillSetOffSkillReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_SET_OFF_SKILL_REQ;

        public JobId Job { get; set; }
        public byte SlotNo { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSkillSetOffSkillReq>
        {
            public override void Write(IBuffer buffer, C2SSkillSetOffSkillReq obj)
            {
                WriteByte(buffer, (byte) obj.Job);
                WriteByte(buffer, obj.SlotNo);
            }

            public override C2SSkillSetOffSkillReq Read(IBuffer buffer)
            {
                return new C2SSkillSetOffSkillReq()
                {
                    Job = (JobId) ReadByte(buffer),
                    SlotNo = ReadByte(buffer)
                };
            }
        }
    }
}