using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnLostPawnWalletReviveRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_LOST_PAWN_WALLET_REVIVE_RES;

        public uint PawnId { get; set; }
        public WalletType Type { get; set; }
        public uint Value { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnLostPawnWalletReviveRes>
        {
            public override void Write(IBuffer buffer, S2CPawnLostPawnWalletReviveRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, (byte)obj.Type);
                WriteUInt32(buffer, obj.Value);
            }

            public override S2CPawnLostPawnWalletReviveRes Read(IBuffer buffer)
            {
                S2CPawnLostPawnWalletReviveRes obj = new S2CPawnLostPawnWalletReviveRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.Type = (WalletType) ReadByte(buffer);
                obj.Value = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}