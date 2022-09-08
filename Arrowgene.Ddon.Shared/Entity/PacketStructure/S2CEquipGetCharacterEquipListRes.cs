using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CEquipGetCharacterEquipListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_EQUIP_GET_CHARACTER_EQUIP_LIST_RES;

        public S2CEquipGetCharacterEquipListRes()
        {
            CharacterEquipList = new List<CDataCharacterEquipInfo>();
            EquipJobItemList = new List<CDataEquipJobItem>();
            PawnEquipItemList = new List<CDataPawnEquipInfo>();
        }
        public List<CDataCharacterEquipInfo> CharacterEquipList { get; set; }
        public List<CDataEquipJobItem> EquipJobItemList { get; set; }
        public List<CDataPawnEquipInfo> PawnEquipItemList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CEquipGetCharacterEquipListRes>
        {
            public override void Write(IBuffer buffer, S2CEquipGetCharacterEquipListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataCharacterEquipInfo>(buffer, obj.CharacterEquipList);
                WriteEntityList<CDataEquipJobItem>(buffer, obj.EquipJobItemList);
                WriteEntityList<CDataPawnEquipInfo>(buffer, obj.PawnEquipItemList);
            }

            public override S2CEquipGetCharacterEquipListRes Read(IBuffer buffer)
            {
                S2CEquipGetCharacterEquipListRes obj = new S2CEquipGetCharacterEquipListRes();
                ReadServerResponse(buffer, obj);
                obj.CharacterEquipList = ReadEntityList<CDataCharacterEquipInfo>(buffer);
                obj.EquipJobItemList = ReadEntityList<CDataEquipJobItem>(buffer);
                obj.PawnEquipItemList = ReadEntityList<CDataPawnEquipInfo>(buffer);
                return obj;
            }
        }
    }
}