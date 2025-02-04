using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnLostPawnWalletReviveReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_LOST_PAWN_WALLET_REVIVE_REQ;

        public uint PawnId { get; set; }
        public WalletType Type { get; set; }
        public uint ReviveCost { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnLostPawnWalletReviveReq>
        {
            public override void Write(IBuffer buffer, C2SPawnLostPawnWalletReviveReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, (byte) obj.Type);
                WriteUInt32(buffer, obj.ReviveCost);
            }

            public override C2SPawnLostPawnWalletReviveReq Read(IBuffer buffer)
            {
                C2SPawnLostPawnWalletReviveReq obj = new C2SPawnLostPawnWalletReviveReq();
                obj.PawnId = ReadUInt32(buffer);
                obj.Type = (WalletType) ReadByte(buffer);
                obj.ReviveCost = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}