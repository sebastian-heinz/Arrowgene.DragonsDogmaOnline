using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillGetPresetAbilityListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_GET_PRESET_ABILITY_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SSkillGetPresetAbilityListReq>
        {
            public override void Write(IBuffer buffer, C2SSkillGetPresetAbilityListReq obj)
            {
            }

            public override C2SSkillGetPresetAbilityListReq Read(IBuffer buffer)
            {
                return new C2SSkillGetPresetAbilityListReq();
            }
        }
    }
}
