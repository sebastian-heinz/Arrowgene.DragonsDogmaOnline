using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillLearnNormalSkillReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_LEARN_NORMAL_SKILL_REQ;

        public JobId Job { get; set; }
        public UInt32 SkillId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSkillLearnNormalSkillReq>
        {
            public override void Write(IBuffer buffer, C2SSkillLearnNormalSkillReq obj)
            {
                WriteByte(buffer, (byte) obj.Job);
                WriteUInt32(buffer, obj.SkillId);
            }

            public override C2SSkillLearnNormalSkillReq Read(IBuffer buffer)
            {
                C2SSkillLearnNormalSkillReq obj = new C2SSkillLearnNormalSkillReq();
                obj.Job = (JobId) ReadByte(buffer);
                obj.SkillId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
