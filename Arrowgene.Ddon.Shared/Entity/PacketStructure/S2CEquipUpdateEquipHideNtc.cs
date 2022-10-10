using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEquipUpdateEquipHideNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_EQUIP_UPDATE_EQUIP_HIDE_NTC;

        public uint CharacterId { get; set; }
        public bool HideHead { get; set; }
        public bool HideLantern { get; set; }
        public bool HidePawnHead { get; set; }
        public bool HidePawnLantern { get; set; }

        public class Serializer : PacketEntitySerializer<S2CEquipUpdateEquipHideNtc>
        {
            public override void Write(IBuffer buffer, S2CEquipUpdateEquipHideNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteBool(buffer, obj.HideHead);
                WriteBool(buffer, obj.HideLantern);
                WriteBool(buffer, obj.HidePawnHead);
                WriteBool(buffer, obj.HidePawnLantern);
            }

            public override S2CEquipUpdateEquipHideNtc Read(IBuffer buffer)
            {
                S2CEquipUpdateEquipHideNtc obj = new S2CEquipUpdateEquipHideNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.HideHead = ReadBool(buffer);
                obj.HideLantern = ReadBool(buffer);
                obj.HidePawnHead = ReadBool(buffer);
                obj.HidePawnLantern = ReadBool(buffer);
                return obj;
            }
        }
    }
}