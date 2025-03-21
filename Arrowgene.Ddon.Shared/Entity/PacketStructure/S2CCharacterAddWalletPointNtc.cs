using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCharacterAddWalletPointNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CHARACTER_ADD_WALLET_POINT_NTC;

        public CDataUpdateWalletPoint UpdateWallet { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CCharacterAddWalletPointNtc>
        {
            public override void Write(IBuffer buffer, S2CCharacterAddWalletPointNtc obj)
            {
                WriteEntity(buffer, obj.UpdateWallet);
            }

            public override S2CCharacterAddWalletPointNtc Read(IBuffer buffer)
            {
                S2CCharacterAddWalletPointNtc obj = new S2CCharacterAddWalletPointNtc();
                obj.UpdateWallet = ReadEntity<CDataUpdateWalletPoint>(buffer);
                return obj;
            }
        }
    }
}
