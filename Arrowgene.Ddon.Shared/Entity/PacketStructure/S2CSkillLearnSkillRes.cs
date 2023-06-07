using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillLearnSkillRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_LEARN_SKILL_RES;

        public JobId Job { get; set; }
        public uint NewJobPoint { get; set; }
        public uint SkillId { get; set; }
        public byte SkillLv { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillLearnSkillRes>
        {
            public override void Write(IBuffer buffer, S2CSkillLearnSkillRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, (byte) obj.Job);
                WriteUInt32(buffer, obj.NewJobPoint);
                WriteUInt32(buffer, obj.SkillId);
                WriteByte(buffer, obj.SkillLv);
            }

            public override S2CSkillLearnSkillRes Read(IBuffer buffer)
            {
                S2CSkillLearnSkillRes obj = new S2CSkillLearnSkillRes();
                ReadServerResponse(buffer, obj);
                obj.Job = (JobId) ReadByte(buffer);
                obj.NewJobPoint = ReadUInt32(buffer);
                obj.SkillId = ReadUInt32(buffer);
                obj.SkillLv = ReadByte(buffer);
                return obj;
            }
        }
    }
}