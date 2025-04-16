using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillGetReleaseAbilityListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_GET_RELEASE_ABILITY_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SSkillGetReleaseAbilityListReq>
        {
            public override void Write(IBuffer buffer, C2SSkillGetReleaseAbilityListReq obj)
            {
            }

            public override C2SSkillGetReleaseAbilityListReq Read(IBuffer buffer)
            {
                return new C2SSkillGetReleaseAbilityListReq();
            }
        }
    }
}
