using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;


namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceGetGatheringItemRes : IPacketStructure
    {
        public S2CInstanceGetGatheringItemRes()
        {
            Req = new C2SInstanceGetGatheringItemReq();
        }

        public S2CInstanceGetGatheringItemRes(C2SInstanceGetGatheringItemReq req)
        {
            Req = req;
        }

        public C2SInstanceGetGatheringItemReq Req { get; set; }
        public PacketId Id => PacketId.S2C_INSTANCE_GET_GATHERING_ITEM_RES;


        public class Serializer : PacketEntitySerializer<S2CInstanceGetGatheringItemRes>
        {

            public override void Write(IBuffer buffer, S2CInstanceGetGatheringItemRes obj)
            {
                C2SInstanceGetGatheringItemReq req = obj.Req;
                    WriteByteArray(buffer, obj.StaticErrorResult);
                    WriteUInt32(buffer, req.StageId);
                    WriteByte(buffer, req.LayerNo);
                    WriteUInt32(buffer, req.GroupId);
                    WriteUInt32(buffer, req.PosId);
                    WriteUInt32(buffer, req.Length);
                    for (uint n = 0; n< req.Length; n++)
                    {
                        WriteUInt32(buffer, n);
                        WriteUInt32(buffer, 0);
                    }
                    WriteByteArray(buffer, obj.Pad6);
            }

            public override S2CInstanceGetGatheringItemRes Read(IBuffer buffer)
            {
                S2CInstanceGetGatheringItemRes obj = new S2CInstanceGetGatheringItemRes();
                return obj;
            }

        }
        private readonly byte[] StaticErrorResult = { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
        private readonly byte[] Pad6 = { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };
    }
}
