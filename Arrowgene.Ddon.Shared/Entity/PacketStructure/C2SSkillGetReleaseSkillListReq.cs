using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillGetReleaseSkillListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_GET_RELEASE_SKILL_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SSkillGetReleaseSkillListReq>
        {
            public override void Write(IBuffer buffer, C2SSkillGetReleaseSkillListReq obj)
            {
            }

            public override C2SSkillGetReleaseSkillListReq Read(IBuffer buffer)
            {
                return new C2SSkillGetReleaseSkillListReq();
            }
        }
    }
}
