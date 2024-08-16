using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterEditPrice
    {    
        public WalletType PointType { get; set; }
        public uint Value { get; set; }
    
        public class Serializer : EntitySerializer<CDataCharacterEditPrice>
        {
            public override void Write(IBuffer buffer, CDataCharacterEditPrice obj)
            {
                WriteByte(buffer, (byte)obj.PointType);
                WriteUInt32(buffer, obj.Value);
            }
        
            public override CDataCharacterEditPrice Read(IBuffer buffer)
            {
                CDataCharacterEditPrice obj = new CDataCharacterEditPrice();
                obj.PointType = (WalletType)ReadByte(buffer);
                obj.Value = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
