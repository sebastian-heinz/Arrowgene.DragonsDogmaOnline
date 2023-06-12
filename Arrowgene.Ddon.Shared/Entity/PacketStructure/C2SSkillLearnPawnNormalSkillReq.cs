using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillLearnPawnNormalSkillReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_LEARN_PAWN_NORMAL_SKILL_REQ;

        public uint PawnId { get; set; }
        public JobId Job { get; set; }
        public uint SkillId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSkillLearnPawnNormalSkillReq>
        {
            public override void Write(IBuffer buffer, C2SSkillLearnPawnNormalSkillReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, (byte) obj.Job);
                WriteUInt32(buffer, obj.SkillId);
            }

            public override C2SSkillLearnPawnNormalSkillReq Read(IBuffer buffer)
            {
                C2SSkillLearnPawnNormalSkillReq obj = new C2SSkillLearnPawnNormalSkillReq();
                obj.PawnId = ReadUInt32(buffer);
                obj.Job = (JobId) ReadByte(buffer);
                obj.SkillId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}