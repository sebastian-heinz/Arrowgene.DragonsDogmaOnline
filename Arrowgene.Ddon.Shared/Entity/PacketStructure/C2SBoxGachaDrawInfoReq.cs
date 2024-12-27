using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBoxGachaDrawInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BOX_GACHA_BOX_GACHA_DRAW_INFO_REQ;

        public uint DrawId { get; set; }

        public C2SBoxGachaDrawInfoReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SBoxGachaDrawInfoReq>
        {
            public override void Write(IBuffer buffer, C2SBoxGachaDrawInfoReq obj)
            {
                WriteUInt32(buffer, obj.DrawId);
            }

            public override C2SBoxGachaDrawInfoReq Read(IBuffer buffer)
            {
                C2SBoxGachaDrawInfoReq obj = new C2SBoxGachaDrawInfoReq();

                obj.DrawId = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
