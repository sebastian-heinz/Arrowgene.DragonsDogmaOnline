using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillGetPawnLearnedNormalSkillListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_GET_PAWN_LEARNED_NORMAL_SKILL_LIST_REQ;

        public uint PawnId { get; set; }        

        public class Serializer : PacketEntitySerializer<C2SSkillGetPawnLearnedNormalSkillListReq>
        {
            public override void Write(IBuffer buffer, C2SSkillGetPawnLearnedNormalSkillListReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }

            public override C2SSkillGetPawnLearnedNormalSkillListReq Read(IBuffer buffer)
            {
                C2SSkillGetPawnLearnedNormalSkillListReq obj = new C2SSkillGetPawnLearnedNormalSkillListReq();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}