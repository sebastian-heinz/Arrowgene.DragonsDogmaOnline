using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillGetLearnedAbilityListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_GET_LEARNED_ABILITY_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SSkillGetLearnedAbilityListReq>
        {
            public override void Write(IBuffer buffer, C2SSkillGetLearnedAbilityListReq obj)
            {
            }

            public override C2SSkillGetLearnedAbilityListReq Read(IBuffer buffer)
            {
                return new C2SSkillGetLearnedAbilityListReq();
            }
        }
    }
}
