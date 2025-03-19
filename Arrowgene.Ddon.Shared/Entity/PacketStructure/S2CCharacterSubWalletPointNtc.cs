using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    // Much the same as S2CCharacterAddWalletPointNtc, but doesn't make a chat print or popup, just sets their wallet amount.
    public class S2CCharacterSubWalletPointNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CHARACTER_SUB_WALLET_POINT_NTC;

        public CDataUpdateWalletPoint UpdateWallet { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CCharacterSubWalletPointNtc>
        {
            public override void Write(IBuffer buffer, S2CCharacterSubWalletPointNtc obj)
            {
                WriteEntity(buffer, obj.UpdateWallet);
            }

            public override S2CCharacterSubWalletPointNtc Read(IBuffer buffer)
            {
                S2CCharacterSubWalletPointNtc obj = new S2CCharacterSubWalletPointNtc();
                obj.UpdateWallet = ReadEntity<CDataUpdateWalletPoint>(buffer);
                return obj;
            }
        }
    }
}
