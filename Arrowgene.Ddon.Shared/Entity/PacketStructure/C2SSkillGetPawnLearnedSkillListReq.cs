using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillGetPawnLearnedSkillListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_GET_PAWN_LEARNED_SKILL_LIST_REQ;

        public uint PawnId { get; set; }        

        public class Serializer : PacketEntitySerializer<C2SSkillGetPawnLearnedSkillListReq>
        {
            public override void Write(IBuffer buffer, C2SSkillGetPawnLearnedSkillListReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }

            public override C2SSkillGetPawnLearnedSkillListReq Read(IBuffer buffer)
            {
                C2SSkillGetPawnLearnedSkillListReq obj = new C2SSkillGetPawnLearnedSkillListReq();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}