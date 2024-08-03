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
            PlusValue = equipItemInfo.PlusValue;
            EquipElementParamList = equipItemInfo.EquipElementParamList;
            AddStatusParamList = equipItemInfo.AddStatusParamList;
        }

        public CDataContextEquipData()
        {
            ItemId=0;
            ColorNo=0;
            PlusValue=0;
            EquipElementParamList=new List<CDataEquipElementParam>();
            AddStatusParamList=new List<CDataAddStatusParam>();
        }

        public ushort ItemId { get; set; }
        public byte ColorNo { get; set; }
        public uint PlusValue { get; set; }
        public List<CDataEquipElementParam> EquipElementParamList { get; set; }
        public List<CDataAddStatusParam> AddStatusParamList { get; set; }

        public class Serializer : EntitySerializer<CDataContextEquipData>
        {
            public override void Write(IBuffer buffer, CDataContextEquipData obj)
            {
                WriteUInt16(buffer, obj.ItemId);
                WriteByte(buffer, obj.ColorNo);
                WriteUInt32(buffer, obj.PlusValue);
                WriteEntityList<CDataEquipElementParam>(buffer, obj.EquipElementParamList);
                WriteEntityList<CDataAddStatusParam>(buffer, obj.AddStatusParamList);
            }

            public override CDataContextEquipData Read(IBuffer buffer)
            {
                CDataContextEquipData obj = new CDataContextEquipData();
                obj.ItemId = ReadUInt16(buffer);
                obj.ColorNo = ReadByte(buffer);
                obj.PlusValue = ReadUInt32(buffer);
                obj.EquipElementParamList = ReadEntityList<CDataEquipElementParam>(buffer);
                obj.AddStatusParamList = ReadEntityList<CDataAddStatusParam>(buffer);
                return obj;
            }
        }
    }
}
