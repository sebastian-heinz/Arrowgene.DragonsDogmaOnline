using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillGetCurrentSetSkillListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_GET_CURRENT_SET_SKILL_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SSkillGetCurrentSetSkillListReq>
        {

            public override void Write(IBuffer buffer, C2SSkillGetCurrentSetSkillListReq obj)
            {
            }

            public override C2SSkillGetCurrentSetSkillListReq Read(IBuffer buffer)
            {
                return new C2SSkillGetCurrentSetSkillListReq();
            }
        }
    }
}
