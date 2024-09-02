using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnSpSkillGetActiveSkillReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_SP_SKILL_GET_ACTIVE_SKILL_REQ;

        public uint PawnId { get; set; }
        public JobId JobId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnSpSkillGetActiveSkillReq>
        {
            public override void Write(IBuffer buffer, C2SPawnSpSkillGetActiveSkillReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, (byte) obj.JobId);
            }

            public override C2SPawnSpSkillGetActiveSkillReq Read(IBuffer buffer)
            {
                C2SPawnSpSkillGetActiveSkillReq obj = new C2SPawnSpSkillGetActiveSkillReq();
                obj.PawnId = ReadUInt32(buffer);
                obj.JobId = (JobId) ReadByte(buffer);
                return obj;
            }
        }

    }
}