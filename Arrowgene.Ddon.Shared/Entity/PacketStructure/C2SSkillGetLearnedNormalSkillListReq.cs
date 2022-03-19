using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillGetLearnedNormalSkillListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_GET_LEARNED_NORMAL_SKILL_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SSkillGetLearnedNormalSkillListReq>
        {
            public override void Write(IBuffer buffer, C2SSkillGetLearnedNormalSkillListReq obj)
            {
            }

            public override C2SSkillGetLearnedNormalSkillListReq Read(IBuffer buffer)
            {
                return new C2SSkillGetLearnedNormalSkillListReq();
            }
        }
    }
}
