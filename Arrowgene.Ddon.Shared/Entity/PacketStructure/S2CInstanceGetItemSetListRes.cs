using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;


namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceGetItemSetListRes : IPacketStructure
    {
        public S2CInstanceGetItemSetListRes()
        {
            Req = new C2SInstanceGetItemSetListReq();
        }

        public S2CInstanceGetItemSetListRes(C2SInstanceGetItemSetListReq req)
        {
            Req = req;
        }

        public C2SInstanceGetItemSetListReq Req { get; set; }
        public PacketId Id => PacketId.S2C_INSTANCE_GET_ITEM_SET_LIST_RES;


        public class Serializer : PacketEntitySerializer<S2CInstanceGetItemSetListRes>
        {

            public override void Write(IBuffer buffer, S2CInstanceGetItemSetListRes obj)
            {
                C2SInstanceGetItemSetListReq req = obj.Req;
                
                WriteByteArray(buffer, obj.Pad8);
                WriteUInt32(buffer, req.StageId);
                WriteByte(buffer, req.LayerNo);
                WriteUInt32(buffer, req.GroupId);
                byte n = 255;
                WriteUInt32(buffer, n);
                for (byte i = 0; i < n; i++)
                {
                    WriteByte(buffer, i);
                    WriteUInt32(buffer, 0);
                }
                WriteUInt16(buffer, 0);
            }

            public override S2CInstanceGetItemSetListRes Read(IBuffer buffer)
            {
                S2CInstanceGetItemSetListRes obj = new S2CInstanceGetItemSetListRes();
                return obj;
            }

        }
        
        private readonly byte[] Pad8 = { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
        
    }
}
