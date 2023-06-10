using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillSetPawnSkillRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_SET_PAWN_SKILL_RES;

        public uint PawnId { get; set; }
        public JobId Job { get; set; }
        public byte SlotNo { get; set; }
        public uint SkillId { get; set; }
        public byte SkillLv { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillSetPawnSkillRes>
        {
            public override void Write(IBuffer buffer, S2CSkillSetPawnSkillRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, (byte) obj.Job);
                WriteByte(buffer, obj.SlotNo);
                WriteUInt32(buffer, obj.SkillId);
                WriteByte(buffer, obj.SkillLv);
            }

            public override S2CSkillSetPawnSkillRes Read(IBuffer buffer)
            {
                S2CSkillSetPawnSkillRes obj = new S2CSkillSetPawnSkillRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.Job = (JobId) ReadByte(buffer);
                obj.SlotNo = ReadByte(buffer);
                obj.SkillId = ReadUInt32(buffer);
                obj.SkillLv = ReadByte(buffer);
                return obj;
            }
        }
    }
}