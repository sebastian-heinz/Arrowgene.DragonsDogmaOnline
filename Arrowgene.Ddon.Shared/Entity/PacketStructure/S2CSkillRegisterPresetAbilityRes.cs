using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillRegisterPresetAbilityRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_REGISTER_PRESET_ABILITY_RES;

        public class Serializer : PacketEntitySerializer<S2CSkillRegisterPresetAbilityRes>
        {
            public override void Write(IBuffer buffer, S2CSkillRegisterPresetAbilityRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CSkillRegisterPresetAbilityRes Read(IBuffer buffer)
            {
                S2CSkillRegisterPresetAbilityRes obj = new S2CSkillRegisterPresetAbilityRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
