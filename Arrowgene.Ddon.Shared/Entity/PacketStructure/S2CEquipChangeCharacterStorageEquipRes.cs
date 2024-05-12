using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEquipChangeCharacterStorageEquipRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_EQUIP_CHANGE_CHARACTER_STORAGE_EQUIP_RES;

        public S2CEquipChangeCharacterStorageEquipRes()
        {
            CharacterEquipList = new List<CDataCharacterEquipInfo>();
            Unk0 = new CDataJobChangeJobResUnk0();
        }

        public List<CDataCharacterEquipInfo> CharacterEquipList { get; set; }
        public CDataJobChangeJobResUnk0 Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CEquipChangeCharacterStorageEquipRes>
        {
            public override void Write(IBuffer buffer, S2CEquipChangeCharacterStorageEquipRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataCharacterEquipInfo>(buffer, obj.CharacterEquipList);
                WriteEntity<CDataJobChangeJobResUnk0>(buffer, obj.Unk0);
            }

            public override S2CEquipChangeCharacterStorageEquipRes Read(IBuffer buffer)
            {
                S2CEquipChangeCharacterStorageEquipRes obj = new S2CEquipChangeCharacterStorageEquipRes();
                ReadServerResponse(buffer, obj);
                obj.CharacterEquipList = ReadEntityList<CDataCharacterEquipInfo>(buffer);
                obj.Unk0 = ReadEntity<CDataJobChangeJobResUnk0>(buffer);
                return obj;
            }
        }
    }
}