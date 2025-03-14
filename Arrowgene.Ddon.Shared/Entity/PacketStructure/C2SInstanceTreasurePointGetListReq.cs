using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceTreasurePointGetListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_TREASURE_POINT_GET_LIST_REQ;

        public uint CategoryId { get; set; }

        public C2SInstanceTreasurePointGetListReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SInstanceTreasurePointGetListReq>
        {
            public override void Write(IBuffer buffer, C2SInstanceTreasurePointGetListReq obj)
            {
                WriteUInt32(buffer, obj.CategoryId);
            }

            public override C2SInstanceTreasurePointGetListReq Read(IBuffer buffer)
            {
                C2SInstanceTreasurePointGetListReq obj = new C2SInstanceTreasurePointGetListReq();
                obj.CategoryId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
