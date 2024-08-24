using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCraftTimeSaveCost
    {
    
        public byte ID { get; set; }
        public WalletType Type { get; set; }
        public uint Price { get; set; }
        public uint Sec { get; set; }
    
        public class Serializer : EntitySerializer<CDataCraftTimeSaveCost>
        {
            public override void Write(IBuffer buffer, CDataCraftTimeSaveCost obj)
            {
                WriteByte(buffer, obj.ID);
                WriteByte(buffer, (byte)obj.Type);
                WriteUInt32(buffer, obj.Price);
                WriteUInt32(buffer, obj.Sec);
            }
        
            public override CDataCraftTimeSaveCost Read(IBuffer buffer)
            {
                CDataCraftTimeSaveCost obj = new CDataCraftTimeSaveCost();
                obj.ID = ReadByte(buffer);
                obj.Type = (WalletType)ReadByte(buffer);
                obj.Price = ReadUInt32(buffer);
                obj.Sec = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
