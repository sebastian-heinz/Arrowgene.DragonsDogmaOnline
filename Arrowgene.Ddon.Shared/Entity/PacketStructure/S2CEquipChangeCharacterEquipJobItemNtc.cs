using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEquipChangeCharacterEquipJobItemNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_EQUIP_CHANGE_CHARACTER_EQUIP_JOB_ITEM_NTC;

        public S2CEquipChangeCharacterEquipJobItemNtc()
        {
            EquipJobItemList = new List<CDataEquipJobItem>();
        }

        public uint CharacterId { get; set; }
        public List<CDataEquipJobItem> EquipJobItemList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CEquipChangeCharacterEquipJobItemNtc>
        {
            public override void Write(IBuffer buffer, S2CEquipChangeCharacterEquipJobItemNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteEntityList<CDataEquipJobItem>(buffer, obj.EquipJobItemList);
            }

            public override S2CEquipChangeCharacterEquipJobItemNtc Read(IBuffer buffer)
            {
                S2CEquipChangeCharacterEquipJobItemNtc obj = new S2CEquipChangeCharacterEquipJobItemNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.EquipJobItemList = ReadEntityList<CDataEquipJobItem>(buffer);
                return obj;
            }
        }

    }
}