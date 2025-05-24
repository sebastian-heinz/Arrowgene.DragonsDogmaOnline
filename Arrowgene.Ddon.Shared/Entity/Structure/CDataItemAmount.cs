using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataItemAmount
    {
        public ItemId ItemId { get; set; }
        public ushort Num { get; set; }
    
        public class Serializer : EntitySerializer<CDataItemAmount>
        {
            public override void Write(IBuffer buffer, CDataItemAmount obj)
            {
                WriteUInt32(buffer, (uint) obj.ItemId);
                WriteUInt16(buffer, obj.Num);
            }
        
            public override CDataItemAmount Read(IBuffer buffer)
            {
                CDataItemAmount obj = new CDataItemAmount();
                obj.ItemId = (ItemId) ReadUInt32(buffer);
                obj.Num = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
