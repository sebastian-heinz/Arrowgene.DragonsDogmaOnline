using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataRequiredItem
    {
        public uint ItemId { get; set; }
        public ushort Num { get; set; }
    
        public class Serializer : EntitySerializer<CDataRequiredItem>
        {
            public override void Write(IBuffer buffer, CDataRequiredItem obj)
            {
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt16(buffer, obj.Num);
            }
        
            public override CDataRequiredItem Read(IBuffer buffer)
            {
                CDataRequiredItem obj = new CDataRequiredItem();
                obj.ItemId = ReadUInt32(buffer);
                obj.Num = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
