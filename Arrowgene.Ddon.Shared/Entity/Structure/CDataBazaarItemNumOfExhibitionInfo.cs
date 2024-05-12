using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBazaarItemNumOfExhibitionInfo
    {
        public uint ItemId { get; set; }
        public ushort Num { get; set; }
    
        public class Serializer : EntitySerializer<CDataBazaarItemNumOfExhibitionInfo>
        {
            public override void Write(IBuffer buffer, CDataBazaarItemNumOfExhibitionInfo obj)
            {
                WriteUInt32(buffer,obj.ItemId);
                WriteUInt16(buffer,obj.Num);
            }
        
            public override CDataBazaarItemNumOfExhibitionInfo Read(IBuffer buffer)
            {
                CDataBazaarItemNumOfExhibitionInfo obj = new CDataBazaarItemNumOfExhibitionInfo();
                obj.ItemId = ReadUInt32(buffer);
                obj.Num = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}