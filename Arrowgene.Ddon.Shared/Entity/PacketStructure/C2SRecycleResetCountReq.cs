using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SRecycleResetCountReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_RECYCLE_RESET_COUNT_REQ;

        public C2SRecycleResetCountReq()
        {
        }

        public byte Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<C2SRecycleResetCountReq>
        {
            public override void Write(IBuffer buffer, C2SRecycleResetCountReq obj)
            {
                WriteByte(buffer, obj.Unk0);
            }

            public override C2SRecycleResetCountReq Read(IBuffer buffer)
            {
                C2SRecycleResetCountReq obj = new C2SRecycleResetCountReq();
                obj.Unk0 = ReadByte(buffer);
                return obj;
            }
        }
    }
}
