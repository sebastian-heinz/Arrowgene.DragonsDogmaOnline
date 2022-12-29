using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGatheringItemListUnk2
    {
        public uint SlotNo { get; set; }
        public uint ItemId { get; set; }
        public uint ItemNum { get; set; }
        public uint Unk3 { get; set; }
        public bool Unk4 { get; set; }

        public class Serializer : EntitySerializer<CDataGatheringItemListUnk2>
        {
            public override void Write(IBuffer buffer, CDataGatheringItemListUnk2 obj)
            {
                WriteUInt32(buffer, obj.SlotNo);
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt32(buffer, obj.ItemNum);
                WriteUInt32(buffer, obj.Unk3);
                WriteBool(buffer, obj.Unk4);
            }
        
            public override CDataGatheringItemListUnk2 Read(IBuffer buffer)
            {
                CDataGatheringItemListUnk2 obj = new CDataGatheringItemListUnk2();
                obj.SlotNo = ReadUInt32(buffer);
                obj.ItemId = ReadUInt32(buffer);
                obj.ItemNum = ReadUInt32(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                obj.Unk4 = ReadBool(buffer);
                return obj;
            }
        }
    }
}