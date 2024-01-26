using Arrowgene.Buffers;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataDeliveredItem
    {
        public uint ItemId { get; set; }
        public ushort ItemNum { get; set; }
        public ushort NeedNum { get; set; }
    
        public class Serializer : EntitySerializer<CDataDeliveredItem>
        {
            public override void Write(IBuffer buffer, CDataDeliveredItem obj)
            {
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt16(buffer, obj.ItemNum);
                WriteUInt16(buffer, obj.NeedNum);
            }
        
            public override CDataDeliveredItem Read(IBuffer buffer)
            {
                CDataDeliveredItem obj = new CDataDeliveredItem();
                obj.ItemId = ReadUInt32(buffer);
                obj.ItemNum = ReadUInt16(buffer);
                obj.NeedNum = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}