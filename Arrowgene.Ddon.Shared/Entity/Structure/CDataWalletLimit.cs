using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataWalletLimit
    {
        public WalletType WalletType { get; set; }
        public uint MaxValue { get; set; }

        public class Serializer : EntitySerializer<CDataWalletLimit>
        {
            public override void Write(IBuffer buffer, CDataWalletLimit obj)
            {
                WriteUInt32(buffer, (uint)obj.WalletType);
                WriteUInt32(buffer, obj.MaxValue);
            }

            public override CDataWalletLimit Read(IBuffer buffer)
            {
                CDataWalletLimit obj = new CDataWalletLimit();
                obj.WalletType = (WalletType)ReadUInt32(buffer);
                obj.MaxValue = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
