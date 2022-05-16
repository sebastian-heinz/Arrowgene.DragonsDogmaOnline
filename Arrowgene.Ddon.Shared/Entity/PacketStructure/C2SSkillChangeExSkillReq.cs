using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillChangeExSkillReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_CHANGE_EX_SKILL_REQ;

        public byte Job { get; set; }
        public uint SkillId { get; set; }
        public uint Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSkillChangeExSkillReq>
        {
            public override void Write(IBuffer buffer, C2SSkillChangeExSkillReq obj)
            {
                WriteByte(buffer, obj.Job);
                WriteUInt32(buffer, obj.SkillId);
                WriteUInt32(buffer, obj.Unk0);
            }

            public override C2SSkillChangeExSkillReq Read(IBuffer buffer)
            {
                C2SSkillChangeExSkillReq obj = new C2SSkillChangeExSkillReq();
                obj.Job = ReadByte(buffer);
                obj.SkillId = ReadUInt32(buffer);
                obj.Unk0 = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}