using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillGetSetSkillListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_GET_SET_SKILL_LIST_REQ;

        public JobId Job { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSkillGetSetSkillListReq>
        {

            public override void Write(IBuffer buffer, C2SSkillGetSetSkillListReq obj)
            {
                WriteByte(buffer, (byte) obj.Job);
            }

            public override C2SSkillGetSetSkillListReq Read(IBuffer buffer)
            {
                return new C2SSkillGetSetSkillListReq() {
                    Job = (JobId) ReadByte(buffer)
                };
            }
        }
    }
}
