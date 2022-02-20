using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillGetAbilityCostReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_GET_ABILITY_COST_REQ;

        public class Serializer : PacketEntitySerializer<C2SSkillGetAbilityCostReq>
        {
            public override void Write(IBuffer buffer, C2SSkillGetAbilityCostReq obj)
            {
            }

            public override C2SSkillGetAbilityCostReq Read(IBuffer buffer)
            {
                return new C2SSkillGetAbilityCostReq();
            }
        }
    }
}
