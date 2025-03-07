using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillSetPresetAbilityNameReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_SET_PRESET_ABILITY_NAME_REQ;

        public byte PresetNo { get; set; }
        public string PresetName { get; set; } = string.Empty;

        public class Serializer : PacketEntitySerializer<C2SSkillSetPresetAbilityNameReq>
        {
            public override void Write(IBuffer buffer, C2SSkillSetPresetAbilityNameReq obj)
            {
                WriteByte(buffer, obj.PresetNo);
                WriteMtString(buffer, obj.PresetName);
            }

            public override C2SSkillSetPresetAbilityNameReq Read(IBuffer buffer)
            {
                C2SSkillSetPresetAbilityNameReq obj = new C2SSkillSetPresetAbilityNameReq();
                obj.PresetNo = ReadByte(buffer);
                obj.PresetName = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
