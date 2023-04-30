using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillSetOffPawnSkillReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_SET_OFF_PAWN_SKILL_REQ;

        public uint PawnId { get; set; }
        public JobId Job { get; set; }
        public byte SlotNo { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSkillSetOffPawnSkillReq>
        {
            public override void Write(IBuffer buffer, C2SSkillSetOffPawnSkillReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, (byte) obj.Job);
                WriteByte(buffer, obj.SlotNo);
            }

            public override C2SSkillSetOffPawnSkillReq Read(IBuffer buffer)
            {
                return new C2SSkillSetOffPawnSkillReq()
                {
                    PawnId = ReadUInt32(buffer),
                    Job = (JobId) ReadByte(buffer),
                    SlotNo = ReadByte(buffer)
                };
            }
        }
    }
}