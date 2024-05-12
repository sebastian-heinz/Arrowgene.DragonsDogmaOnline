using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillLearnSkillReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_LEARN_SKILL_REQ;

        public JobId Job { get; set; }
        public uint SkillId { get; set; }
        public byte SkillLv { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSkillLearnSkillReq>
        {
            public override void Write(IBuffer buffer, C2SSkillLearnSkillReq obj)
            {
                WriteByte(buffer, (byte) obj.Job);
                WriteUInt32(buffer, obj.SkillId);
                WriteByte(buffer, obj.SkillLv);
            }

            public override C2SSkillLearnSkillReq Read(IBuffer buffer)
            {
                C2SSkillLearnSkillReq obj = new C2SSkillLearnSkillReq();
                obj.Job = (JobId) ReadByte(buffer);
                obj.SkillId = ReadUInt32(buffer);
                obj.SkillLv = ReadByte(buffer);
                return obj;
            }
        }

    }
}