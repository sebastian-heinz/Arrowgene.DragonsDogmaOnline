using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceGetGatheringItemListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_GET_GATHERING_ITEM_LIST_REQ;
        
        public uint StageId { get; set; }
        public byte LayerNo { get; set; }
        public uint GroupId { get; set; }
        public uint PosId { get; set; }

        public C2SInstanceGetGatheringItemListReq()
        {
            StageId = 0;
            LayerNo = 0;
            GroupId = 0;
            PosId = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SInstanceGetGatheringItemListReq>
        {
            public override void Write(IBuffer buffer, C2SInstanceGetGatheringItemListReq obj)
            {
                WriteUInt32(buffer, obj.StageId);
                WriteByte(buffer, obj.LayerNo);
                WriteUInt32(buffer, obj.GroupId);
                WriteUInt32(buffer, obj.PosId);
            }

            public override C2SInstanceGetGatheringItemListReq Read(IBuffer buffer)
            {
                C2SInstanceGetGatheringItemListReq obj = new C2SInstanceGetGatheringItemListReq();
                obj.StageId = ReadUInt32(buffer);
                obj.LayerNo = ReadByte(buffer);
                obj.GroupId = ReadUInt32(buffer);
                obj.PosId = ReadUInt32(buffer);

                return obj;
            }
        }
    }
}
