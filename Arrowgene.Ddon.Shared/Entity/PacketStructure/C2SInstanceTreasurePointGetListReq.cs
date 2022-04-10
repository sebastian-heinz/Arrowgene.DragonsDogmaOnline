using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceTreasurePointGetListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_TREASURE_POINT_GET_LIST_REQ;

        public uint Data0 { get; set; }

        public C2SInstanceTreasurePointGetListReq()
        {
            Data0 = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SInstanceTreasurePointGetListReq>
        {
            public override void Write(IBuffer buffer, C2SInstanceTreasurePointGetListReq obj)
            {
                WriteUInt32(buffer, obj.Data0);
            }

            public override C2SInstanceTreasurePointGetListReq Read(IBuffer buffer)
            {
                C2SInstanceTreasurePointGetListReq obj = new C2SInstanceTreasurePointGetListReq();
                obj.Data0 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
