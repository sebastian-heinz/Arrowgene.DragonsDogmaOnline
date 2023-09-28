using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillLearnNormalSkillRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_LEARN_NORMAL_SKILL_RES;

        public JobId Job { get; set; }
        public uint SkillId { get; set; }
        public uint NewJobPoint { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillLearnNormalSkillRes>
        {
            public override void Write(IBuffer buffer, S2CSkillLearnNormalSkillRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, (byte) obj.Job);
                WriteUInt32(buffer, obj.SkillId);
                WriteUInt32(buffer, obj.NewJobPoint);
            }

            public override S2CSkillLearnNormalSkillRes Read(IBuffer buffer)
            {
                S2CSkillLearnNormalSkillRes obj = new S2CSkillLearnNormalSkillRes();
                ReadServerResponse(buffer, obj);
                obj.Job = (JobId) ReadByte(buffer);
                obj.SkillId = ReadUInt32(buffer);
                obj.NewJobPoint = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}