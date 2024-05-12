using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPawnEquipInfo
    {
        public CDataPawnEquipInfo()
        {
            CharacterEquipList = new List<CDataCharacterEquipInfo>();
            EquipJobItemList = new List<CDataEquipJobItem>();
        }

        public uint PawnId { get; set; }
        public List<CDataCharacterEquipInfo> CharacterEquipList { get; set; }
        public List<CDataEquipJobItem> EquipJobItemList { get; set; }

        public class Serializer : EntitySerializer<CDataPawnEquipInfo>
        {
            public override void Write(IBuffer buffer, CDataPawnEquipInfo obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList<CDataCharacterEquipInfo>(buffer, obj.CharacterEquipList);
                WriteEntityList<CDataEquipJobItem>(buffer, obj.EquipJobItemList);
            }

            public override CDataPawnEquipInfo Read(IBuffer buffer)
            {
                CDataPawnEquipInfo obj = new CDataPawnEquipInfo();
                obj.PawnId = ReadUInt32(buffer);
                obj.CharacterEquipList = ReadEntityList<CDataCharacterEquipInfo>(buffer);
                obj.EquipJobItemList = ReadEntityList<CDataEquipJobItem>(buffer);
                return obj;
            }
        }
    }
}