using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataUpdateWalletPoint
    {
        public CDataUpdateWalletPoint()
        {
            Type=0;
            Value=0;
            AddPoint=0;
            ExtraBonusPoint=0;
        }

        public WalletType Type { get; set; }
        public uint Value { get; set; }
        public int AddPoint { get; set; }
        public uint ExtraBonusPoint { get; set; }

        public class Serializer : EntitySerializer<CDataUpdateWalletPoint>
        {
            public override void Write(IBuffer buffer, CDataUpdateWalletPoint obj)
            {
                WriteByte(buffer, (byte) obj.Type);
                WriteUInt32(buffer, obj.Value);
                WriteInt32(buffer, obj.AddPoint);
                WriteUInt32(buffer, obj.ExtraBonusPoint);
            }

            public override CDataUpdateWalletPoint Read(IBuffer buffer)
            {
                CDataUpdateWalletPoint obj = new CDataUpdateWalletPoint();
                obj.Type = (WalletType) ReadByte(buffer);
                obj.Value = ReadUInt32(buffer);
                obj.AddPoint = ReadInt32(buffer);
                obj.ExtraBonusPoint = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
