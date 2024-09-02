using Arrowgene.Buffers;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataQuestKeyItemPoint
    {
        public byte PointID { get; set; }
        public ushort Point { get; set; }
    
        public class Serializer : EntitySerializer<CDataQuestKeyItemPoint>
        {
            public override void Write(IBuffer buffer, CDataQuestKeyItemPoint obj)
            {
                WriteByte(buffer, obj.PointID);
                WriteUInt16(buffer, obj.Point);
            }
        
            public override CDataQuestKeyItemPoint Read(IBuffer buffer)
            {
                CDataQuestKeyItemPoint obj = new CDataQuestKeyItemPoint();
                obj.PointID = ReadByte(buffer);
                obj.Point = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}