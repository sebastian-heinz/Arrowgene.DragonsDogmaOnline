using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGatheringItemElement
    {
        public uint SlotNo { get; set; }
        public uint ItemId { get; set; }
        public uint ItemNum { get; set; }
        public uint Quality { get; set; }
        public bool IsHidden { get; set; }

        public class Serializer : EntitySerializer<CDataGatheringItemElement>
        {
            public override void Write(IBuffer buffer, CDataGatheringItemElement obj)
            {
                WriteUInt32(buffer, obj.SlotNo);
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt32(buffer, obj.ItemNum);
                WriteUInt32(buffer, obj.Quality);
                WriteBool(buffer, obj.IsHidden);
            }
        
            public override CDataGatheringItemElement Read(IBuffer buffer)
            {
                CDataGatheringItemElement obj = new CDataGatheringItemElement();
                obj.SlotNo = ReadUInt32(buffer);
                obj.ItemId = ReadUInt32(buffer);
                obj.ItemNum = ReadUInt32(buffer);
                obj.Quality = ReadUInt32(buffer);
                obj.IsHidden = ReadBool(buffer);
                return obj;
            }
        }
    }
}