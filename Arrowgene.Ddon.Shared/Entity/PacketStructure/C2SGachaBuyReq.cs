using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SGachaBuyReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_GACHA_GACHA_BUY_REQ;

        public uint GachaId { get; set; }
        public uint DrawGroupId { get; set; }
        public uint SettlementId { get; set; }
        public uint Price { get; set; }

        public C2SGachaBuyReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SGachaBuyReq>
        {
            public override void Write(IBuffer buffer, C2SGachaBuyReq obj)
            {
                WriteUInt32(buffer, obj.GachaId);
                WriteUInt32(buffer, obj.DrawGroupId);
                WriteUInt32(buffer, obj.SettlementId);
                WriteUInt32(buffer, obj.Price);
            }

            public override C2SGachaBuyReq Read(IBuffer buffer)
            {
                C2SGachaBuyReq obj = new C2SGachaBuyReq();

                obj.GachaId = ReadUInt32(buffer);
                obj.DrawGroupId = ReadUInt32(buffer);
                obj.SettlementId = ReadUInt32(buffer);
                obj.Price = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
