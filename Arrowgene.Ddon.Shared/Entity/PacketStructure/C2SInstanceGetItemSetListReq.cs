using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceGetItemSetListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_GET_ITEM_SET_LIST_REQ;
        
        public uint StageId { get; set; }
        public byte LayerNo { get; set; }
        public uint GroupId { get; set; }
        
        public C2SInstanceGetItemSetListReq()
        {
            StageId = 0;
            LayerNo = 0;
            GroupId = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SInstanceGetItemSetListReq>
        {
            public override void Write(IBuffer buffer, C2SInstanceGetItemSetListReq obj)
            {
                WriteUInt32(buffer, obj.StageId);
                WriteByte(buffer, obj.LayerNo);
                WriteUInt32(buffer, obj.GroupId);
            }

            public override C2SInstanceGetItemSetListReq Read(IBuffer buffer)
            {
                C2SInstanceGetItemSetListReq obj = new C2SInstanceGetItemSetListReq();
                obj.StageId = ReadUInt32(buffer);
                obj.LayerNo = ReadByte(buffer);
                obj.GroupId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
