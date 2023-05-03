using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillSetOffPawnSkillRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_SET_OFF_PAWN_SKILL_RES;

        public uint PawnId { get; set; }
        public JobId Job { get; set; }
        public byte SlotNo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillSetOffPawnSkillRes>
        {
            public override void Write(IBuffer buffer, S2CSkillSetOffPawnSkillRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, (byte) obj.Job);
                WriteByte(buffer, obj.SlotNo);
            }

            public override S2CSkillSetOffPawnSkillRes Read(IBuffer buffer)
            {
                S2CSkillSetOffPawnSkillRes obj = new S2CSkillSetOffPawnSkillRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.Job = (JobId) ReadByte(buffer);
                obj.SlotNo = ReadByte(buffer);
                return obj;
            }
        }
    }
}