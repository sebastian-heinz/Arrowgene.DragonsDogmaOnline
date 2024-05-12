using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataWalletPoint
    {
        public WalletType Type { get; set; }
        public uint Value { get; set; }

        public CDataUpdateWalletPoint ToCDataUpdateWalletPoint(uint CurrentAmount, uint BonusAmount = 0)
        {
            return new CDataUpdateWalletPoint()
            {
                Type = Type,
                AddPoint = (int) Value,
                ExtraBonusPoint = BonusAmount,
                Value = CurrentAmount
            };
        }

        public class Serializer : EntitySerializer<CDataWalletPoint>
        {
            public override void Write(IBuffer buffer, CDataWalletPoint obj)
            {
                WriteByte(buffer, (byte) obj.Type);
                WriteUInt32(buffer, obj.Value);
            }

            public override CDataWalletPoint Read(IBuffer buffer)
            {
                CDataWalletPoint obj = new CDataWalletPoint();
                obj.Type = (WalletType) ReadByte(buffer);
                obj.Value = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
