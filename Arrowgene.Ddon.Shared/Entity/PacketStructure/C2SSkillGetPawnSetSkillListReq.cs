using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillGetPawnSetSkillListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_GET_PAWN_SET_SKILL_LIST_REQ;

        public uint PawnId { get; set; }
        public JobId Job { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSkillGetPawnSetSkillListReq>
        {
            public override void Write(IBuffer buffer, C2SSkillGetPawnSetSkillListReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, (byte) obj.Job);
            }

            public override C2SSkillGetPawnSetSkillListReq Read(IBuffer buffer)
            {
                C2SSkillGetPawnSetSkillListReq obj = new C2SSkillGetPawnSetSkillListReq();
                obj.PawnId = ReadUInt32(buffer);
                obj.Job = (JobId) ReadByte(buffer);
                return obj;
            }
        }

    }
}