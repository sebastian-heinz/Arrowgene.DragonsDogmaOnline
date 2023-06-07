using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillSetPawnSkillReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_SET_PAWN_SKILL_REQ;

        public uint PawnId { get; set; }
        public JobId Job { get; set; }
        public byte SlotNo { get; set; }
        public uint SkillId { get; set; }
        public byte SkillLv { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSkillSetPawnSkillReq>
        {
            public override void Write(IBuffer buffer, C2SSkillSetPawnSkillReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, (byte) obj.Job);
                WriteByte(buffer, obj.SlotNo);
                WriteUInt32(buffer, obj.SkillId);
                WriteByte(buffer, obj.SkillLv);
            }

            public override C2SSkillSetPawnSkillReq Read(IBuffer buffer)
            {
                C2SSkillSetPawnSkillReq obj = new C2SSkillSetPawnSkillReq();
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