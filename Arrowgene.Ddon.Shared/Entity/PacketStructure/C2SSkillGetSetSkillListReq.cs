using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillGetSetSkillListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_GET_SET_SKILL_LIST_REQ;

        public class Serializer : EntitySerializer<C2SSkillGetSetSkillListReq>
        {
            public override void Write(IBuffer buffer, C2SSkillGetSetSkillListReq obj)
            {
            }

            public override C2SSkillGetSetSkillListReq Read(IBuffer buffer)
            {
                return new C2SSkillGetSetSkillListReq();
            }
        }
    }
}