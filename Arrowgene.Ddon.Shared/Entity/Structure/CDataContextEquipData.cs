using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataContextEquipData
    {
        public CDataContextEquipData(CDataEquipItemInfo equipItemInfo)
        {
            ItemId = (ushort) equipItemInfo.ItemId;
            ColorNo = equipItemInfo.Color;
            // QualityParam?
            WeaponCrestDataList = equipItemInfo.EquipElementParamList;
            ArmorCrestDataList = equipItemInfo.Unk1;
        }

        public CDataContextEquipData()
        {
            ItemId=0;
            ColorNo=0;
            QualityParam=0;
            WeaponCrestDataList=new List<CDataEquipElementParam>();
            ArmorCrestDataList=new List<CDataEquipItemInfoUnk1>();
        }

        public ushort ItemId { get; set; }
        public byte ColorNo { get; set; }
        public uint QualityParam { get; set; }
        public List<CDataEquipElementParam> WeaponCrestDataList { get; set; }
        public List<CDataEquipItemInfoUnk1> ArmorCrestDataList { get; set; }

        public class Serializer : EntitySerializer<CDataContextEquipData>
        {
            public override void Write(IBuffer buffer, CDataContextEquipData obj)
            {
                WriteUInt16(buffer, obj.ItemId);
                WriteByte(buffer, obj.ColorNo);
                WriteUInt32(buffer, obj.QualityParam);
                WriteEntityList<CDataEquipElementParam>(buffer, obj.WeaponCrestDataList);
                WriteEntityList<CDataEquipItemInfoUnk1>(buffer, obj.ArmorCrestDataList);
            }

            public override CDataContextEquipData Read(IBuffer buffer)
            {
                CDataContextEquipData obj = new CDataContextEquipData();
                obj.ItemId = ReadUInt16(buffer);
                obj.ColorNo = ReadByte(buffer);
                obj.QualityParam = ReadUInt32(buffer);
                obj.WeaponCrestDataList = ReadEntityList<CDataEquipElementParam>(buffer);
                obj.ArmorCrestDataList = ReadEntityList<CDataEquipItemInfoUnk1>(buffer);
                return obj;
            }
        }
    }
}
