using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataContextEquipJobItemData
    {
        public CDataContextEquipJobItemData(CDataEquipJobItem equipJobItem)
        {
            ItemId = (ushort) equipJobItem.JobItemId;
            EquipSlotNo = equipJobItem.EquipSlotNo;
        }

        public CDataContextEquipJobItemData()
        {
            ItemId=0;
            EquipSlotNo=0;
        }

        public static List<CDataContextEquipJobItemData> FromCDataEquipJobItems(List<CDataEquipJobItem> equipJobItems)
        {
            List<CDataContextEquipJobItemData> obj = new List<CDataContextEquipJobItemData>();
            foreach (var equipJobItem in equipJobItems)
            {
                obj.Add(new CDataContextEquipJobItemData(equipJobItem));
            }

            return obj;
        }

        public ushort ItemId { get; set; }
        public byte EquipSlotNo { get; set; }

        public class Serializer : EntitySerializer<CDataContextEquipJobItemData>
        {
            public override void Write(IBuffer buffer, CDataContextEquipJobItemData obj)
            {
                WriteUInt16(buffer, obj.ItemId);
                WriteByte(buffer, obj.EquipSlotNo);
            }

            public override CDataContextEquipJobItemData Read(IBuffer buffer)
            {
                CDataContextEquipJobItemData obj = new CDataContextEquipJobItemData();
                obj.ItemId = ReadUInt16(buffer);
                obj.EquipSlotNo = ReadByte(buffer);
                return obj;
            }
        }
    }
}
