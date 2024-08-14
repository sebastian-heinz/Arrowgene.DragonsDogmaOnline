using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBoxGachaBuyReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BOX_GACHA_BOX_GACHA_BUY_REQ;

        public uint BoxGachaId { get; set; }
        public uint DrawId { get; set; }
        public uint SettlementId { get; set; }
        public uint Price { get; set; }

        public C2SBoxGachaBuyReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SBoxGachaBuyReq>
        {
            public override void Write(IBuffer buffer, C2SBoxGachaBuyReq obj)
            {
                WriteUInt32(buffer, obj.BoxGachaId);
                WriteUInt32(buffer, obj.DrawId);
                WriteUInt32(buffer, obj.SettlementId);
                WriteUInt32(buffer, obj.Price);
            }

            public override C2SBoxGachaBuyReq Read(IBuffer buffer)
            {
                C2SBoxGachaBuyReq obj = new C2SBoxGachaBuyReq();

                obj.BoxGachaId = ReadUInt32(buffer);
                obj.DrawId = ReadUInt32(buffer);
                obj.SettlementId = ReadUInt32(buffer);
                obj.Price = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
