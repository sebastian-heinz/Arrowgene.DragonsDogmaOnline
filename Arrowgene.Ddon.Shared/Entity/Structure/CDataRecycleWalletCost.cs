using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataRecycleWalletCost
    {
        public uint Unk0 { get; set; }
        public WalletType WalletType { get; set; }
        public uint Amount { get; set; }

        public class Serializer : EntitySerializer<CDataRecycleWalletCost>
        {
            public override void Write(IBuffer buffer, CDataRecycleWalletCost obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteByte(buffer, (byte) obj.WalletType);
                WriteUInt32(buffer, obj.Amount);
            }

            public override CDataRecycleWalletCost Read(IBuffer buffer)
            {
                CDataRecycleWalletCost obj = new CDataRecycleWalletCost();
                obj.Unk0 = ReadUInt32(buffer);
                obj.WalletType = (WalletType) ReadByte(buffer);
                obj.Amount = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
