using Arrowgene.Buffers;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataDispelBaseItem
    {
        public CDataDispelBaseItem()
        {
            BaseItemId = new List<CDataDispelBaseItemData>();
            Cost = new List<CDataWalletPoint>();
            LotItemList = new List<CDataDispelLotData>();
            Label = "Test";
        }

        public uint Id { get; set; }
        public uint Unk0 { get; set; }
        public ulong Begin { get; set; }
        public ulong End { get; set; }
        public List<CDataDispelBaseItemData> BaseItemId {  get; set; }
        public List<CDataWalletPoint> Cost {  get; set; }
        public bool IsHide { get; set; }
        public List<CDataDispelLotData> LotItemList { get; set; }
        public uint SortId {  get; set; }
        public string Label {  get; set; }
        public uint Category { get; set; }
        public ushort Unk2 { get; set; }
        public ushort Unk3 {  get; set; }

        public class Serializer : EntitySerializer<CDataDispelBaseItem>
        {
            public override void Write(IBuffer buffer, CDataDispelBaseItem obj)
            {
                WriteUInt32(buffer, obj.Id);
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt64(buffer, obj.Begin);
                WriteUInt64(buffer, obj.End);
                WriteEntityList(buffer, obj.BaseItemId);
                WriteEntityList(buffer, obj.Cost);
                WriteBool(buffer, obj.IsHide);
                WriteEntityList(buffer, obj.LotItemList);
                WriteUInt32(buffer, obj.SortId);
                WriteMtString(buffer, obj.Label);
                WriteUInt32(buffer, obj.Category);
                WriteUInt16(buffer, obj.Unk2);
                WriteUInt16(buffer, obj.Unk3);
            }

            public override CDataDispelBaseItem Read(IBuffer buffer)
            {
                CDataDispelBaseItem obj = new CDataDispelBaseItem();
                obj.Id = ReadUInt32(buffer);
                obj.Unk0 = ReadUInt32(buffer);
                obj.Begin = ReadUInt64(buffer);
                obj.End = ReadUInt64(buffer);
                obj.BaseItemId = ReadEntityList<CDataDispelBaseItemData>(buffer);
                obj.Cost = ReadEntityList<CDataWalletPoint>(buffer);
                obj.IsHide = ReadBool(buffer);
                obj.LotItemList = ReadEntityList<CDataDispelLotData>(buffer);
                obj.SortId = ReadUInt32(buffer);
                obj.Label = ReadMtString(buffer);
                obj.Category = ReadUInt32(buffer);
                obj.Unk2 = ReadUInt16(buffer);
                obj.Unk3 = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}

