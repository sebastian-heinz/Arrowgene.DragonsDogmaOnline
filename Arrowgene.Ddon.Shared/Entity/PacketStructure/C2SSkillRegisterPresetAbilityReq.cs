using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillRegisterPresetAbilityReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_REGISTER_PRESET_ABILITY_REQ;

        public uint PawnId { get; set; } //???
        public byte PresetNo { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSkillRegisterPresetAbilityReq>
        {
            public override void Write(IBuffer buffer, C2SSkillRegisterPresetAbilityReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, obj.PresetNo);
            }

            public override C2SSkillRegisterPresetAbilityReq Read(IBuffer buffer)
            {
                C2SSkillRegisterPresetAbilityReq obj = new C2SSkillRegisterPresetAbilityReq();
                obj.PawnId = ReadUInt32(buffer);
                obj.PresetNo = ReadByte(buffer);
                return obj;
            }
        }
    }
}
