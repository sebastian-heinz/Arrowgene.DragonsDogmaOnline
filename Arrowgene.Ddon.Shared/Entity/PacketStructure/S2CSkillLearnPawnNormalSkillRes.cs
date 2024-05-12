using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillLearnPawnNormalSkillRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_LEARN_PAWN_NORMAL_SKILL_RES;

        public uint PawnId { get; set; }
        public JobId Job { get; set; }
        public uint SkillIndex { get; set; }
        public uint NewJobPoint { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillLearnPawnNormalSkillRes>
        {
            public override void Write(IBuffer buffer, S2CSkillLearnPawnNormalSkillRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, (byte)obj.Job);
                WriteUInt32(buffer, obj.SkillIndex);
                WriteUInt32(buffer, obj.NewJobPoint);
            }

            public override S2CSkillLearnPawnNormalSkillRes Read(IBuffer buffer)
            {
                S2CSkillLearnPawnNormalSkillRes obj = new S2CSkillLearnPawnNormalSkillRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.Job = (JobId)ReadByte(buffer);
                obj.SkillIndex = ReadUInt32(buffer);
                obj.NewJobPoint = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
