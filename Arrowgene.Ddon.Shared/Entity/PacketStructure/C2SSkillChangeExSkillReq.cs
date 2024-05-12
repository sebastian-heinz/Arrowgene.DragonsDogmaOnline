using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillChangeExSkillReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_CHANGE_EX_SKILL_REQ;

        public JobId Job { get; set; }
        public uint SkillId { get; set; }
        public uint PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSkillChangeExSkillReq>
        {
            public override void Write(IBuffer buffer, C2SSkillChangeExSkillReq obj)
            {
                WriteByte(buffer, (byte) obj.Job);
                WriteUInt32(buffer, obj.SkillId);
                WriteUInt32(buffer, obj.PawnId);
            }

            public override C2SSkillChangeExSkillReq Read(IBuffer buffer)
            {
                C2SSkillChangeExSkillReq obj = new C2SSkillChangeExSkillReq();
                obj.Job = (JobId) ReadByte(buffer);
                obj.SkillId = ReadUInt32(buffer);
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}