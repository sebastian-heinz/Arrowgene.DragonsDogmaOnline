using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SRecycleGetInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_RECYCLE_GET_INFO_REQ;

        public bool Unk0 { get; set; }

        public class Serializer : PacketEntitySerializer<C2SRecycleGetInfoReq>
        {
            public override void Write(IBuffer buffer, C2SRecycleGetInfoReq obj)
            {
                WriteBool(buffer, obj.Unk0);
            }

            public override C2SRecycleGetInfoReq Read(IBuffer buffer)
            {
                C2SRecycleGetInfoReq obj = new C2SRecycleGetInfoReq();
                obj.Unk0 = ReadBool(buffer);
                return obj;
            }
        }
    }
}
