using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SEquipUpdateEquipPresetReq : IPacketStructure
    {
        public C2SEquipUpdateEquipPresetReq()
        {
            PresetName = string.Empty;
        }

        public PacketId Id => PacketId.C2S_EQUIP_UPDATE_EQUIP_PRESET_REQ;

        public uint PresetNo { get; set; }
        public uint PawnId { get; set; }
        public uint Type { get; set; }
        public string PresetName { get; set; }

        public class Serializer : PacketEntitySerializer<C2SEquipUpdateEquipPresetReq>
        {
            public override void Write(IBuffer buffer, C2SEquipUpdateEquipPresetReq obj)
            {
                WriteUInt32(buffer, obj.PresetNo);
                WriteUInt32(buffer, obj.PawnId);
                WriteUInt32(buffer, obj.Type);
                WriteMtString(buffer, obj.PresetName);
            }

            public override C2SEquipUpdateEquipPresetReq Read(IBuffer buffer)
            {
                C2SEquipUpdateEquipPresetReq obj = new C2SEquipUpdateEquipPresetReq();
                obj.PresetNo = ReadUInt32(buffer);
                obj.PawnId = ReadUInt32(buffer);
                obj.Type = ReadUInt32(buffer);
                obj.PresetName = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
