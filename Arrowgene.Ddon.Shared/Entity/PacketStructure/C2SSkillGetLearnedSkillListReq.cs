using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillGetLearnedSkillListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_GET_LEARNED_SKILL_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SSkillGetLearnedSkillListReq>
        {

            public override void Write(IBuffer buffer, C2SSkillGetLearnedSkillListReq obj)
            {
            }

            public override C2SSkillGetLearnedSkillListReq Read(IBuffer buffer)
            {
                return new C2SSkillGetLearnedSkillListReq();
            }
        }
    }
}
