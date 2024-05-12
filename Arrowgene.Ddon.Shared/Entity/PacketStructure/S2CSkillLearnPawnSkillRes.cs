using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillLearnPawnSkillRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_LEARN_PAWN_SKILL_RES;

        public uint PawnId { get; set; }
        public JobId Job { get; set; }
        public uint NewJobPoint { get; set; }
        public uint SkillId { get; set; }
        public byte SkillLv { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillLearnPawnSkillRes>
        {
            public override void Write(IBuffer buffer, S2CSkillLearnPawnSkillRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, (byte) obj.Job);
                WriteUInt32(buffer, obj.NewJobPoint);
                WriteUInt32(buffer, obj.SkillId);
                WriteByte(buffer, obj.SkillLv);
            }

            public override S2CSkillLearnPawnSkillRes Read(IBuffer buffer)
            {
                S2CSkillLearnPawnSkillRes obj = new S2CSkillLearnPawnSkillRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.Job = (JobId) ReadByte(buffer);
                obj.NewJobPoint = ReadUInt32(buffer);
                obj.SkillId = ReadUInt32(buffer);
                obj.SkillLv = ReadByte(buffer);
                return obj;
            }
        }
    }
}