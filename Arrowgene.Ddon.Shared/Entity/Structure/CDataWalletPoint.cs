using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataWalletPoint
    {
        public byte Type;
        public uint Value;
    
        public class Serializer : EntitySerializer<CDataWalletPoint>
        {
            public override void Write(IBuffer buffer, CDataWalletPoint obj)
            {
                WriteByte(buffer, obj.Type);
                WriteUInt32(buffer, obj.Value);
            }

            public override CDataWalletPoint Read(IBuffer buffer)
            {
                CDataWalletPoint obj = new CDataWalletPoint();
                obj.Type = ReadByte(buffer);
                obj.Value = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
