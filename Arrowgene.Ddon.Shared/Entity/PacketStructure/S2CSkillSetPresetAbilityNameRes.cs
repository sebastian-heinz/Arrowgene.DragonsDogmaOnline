using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillSetPresetAbilityNameRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_SET_PRESET_ABILITY_NAME_RES;

        public class Serializer : PacketEntitySerializer<S2CSkillSetPresetAbilityNameRes>
        {
            public override void Write(IBuffer buffer, S2CSkillSetPresetAbilityNameRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CSkillSetPresetAbilityNameRes Read(IBuffer buffer)
            {
                S2CSkillSetPresetAbilityNameRes obj = new S2CSkillSetPresetAbilityNameRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
