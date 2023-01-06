using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEquipChangeCharacterEquipNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_EQUIP_CHANGE_CHARACTER_EQUIP_NTC;

        public S2CEquipChangeCharacterEquipNtc()
        {
            EquipItemList = new List<CDataEquipItemInfo>();
            VisualEquipItemList = new List<CDataEquipItemInfo>();
            Unk0 = new CDataJobChangeJobResUnk0();
        }

        public uint CharacterId;
        public List<CDataEquipItemInfo> EquipItemList;
        public List<CDataEquipItemInfo> VisualEquipItemList;
        public CDataJobChangeJobResUnk0 Unk0;

        public class Serializer : PacketEntitySerializer<S2CEquipChangeCharacterEquipNtc>
        {
            public override void Write(IBuffer buffer, S2CEquipChangeCharacterEquipNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteEntityList<CDataEquipItemInfo>(buffer, obj.EquipItemList);
                WriteEntityList<CDataEquipItemInfo>(buffer, obj.VisualEquipItemList);
                WriteEntity<CDataJobChangeJobResUnk0>(buffer, obj.Unk0);
            }

            public override S2CEquipChangeCharacterEquipNtc Read(IBuffer buffer)
            {
                S2CEquipChangeCharacterEquipNtc obj = new S2CEquipChangeCharacterEquipNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.EquipItemList = ReadEntityList<CDataEquipItemInfo>(buffer);
                obj.VisualEquipItemList = ReadEntityList<CDataEquipItemInfo>(buffer);
                obj.Unk0 = ReadEntity<CDataJobChangeJobResUnk0>(buffer);
                return obj;
            }
        }

    }
}