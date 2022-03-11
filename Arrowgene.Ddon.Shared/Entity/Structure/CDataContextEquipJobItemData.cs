using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataContextEquipJobItemData
    {
        public CDataContextEquipJobItemData()
        {
            ItemID=0;
            EquipSlotNo=0;
        }
        
        public ushort ItemID { get; set; }
        public byte EquipSlotNo { get; set; }

        public class Serializer : EntitySerializer<CDataContextEquipJobItemData>
        {
            public override void Write(IBuffer buffer, CDataContextEquipJobItemData obj)
            {
                WriteUInt16(buffer, obj.ItemID);
                WriteByte(buffer, obj.EquipSlotNo);
            }

            public override CDataContextEquipJobItemData Read(IBuffer buffer)
            {
                CDataContextEquipJobItemData obj = new CDataContextEquipJobItemData();
                obj.ItemID = ReadUInt16(buffer);
                obj.EquipSlotNo = ReadByte(buffer);
                return obj;
            }
        }
    }
}
