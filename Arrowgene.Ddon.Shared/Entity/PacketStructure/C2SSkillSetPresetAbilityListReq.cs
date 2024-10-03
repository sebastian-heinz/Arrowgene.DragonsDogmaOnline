using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillSetPresetAbilityListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_SET_PRESET_ABILITY_LIST_REQ;

        public uint PawnId { get; set; } //???
        public byte PresetNo { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSkillSetPresetAbilityListReq>
        {
            public override void Write(IBuffer buffer, C2SSkillSetPresetAbilityListReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, obj.PresetNo);
            }

            public override C2SSkillSetPresetAbilityListReq Read(IBuffer buffer)
            {
                C2SSkillSetPresetAbilityListReq obj = new C2SSkillSetPresetAbilityListReq();
                obj.PawnId = ReadUInt32(buffer);
                obj.PresetNo = ReadByte(buffer);
                return obj;
            }
        }
    }
}
