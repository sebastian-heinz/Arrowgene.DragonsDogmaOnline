using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceSetOmInstantKeyValueRes : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_INSTANCE_SET_OM_INSTANT_KEY_VALUE_RES;
        
        public S2CInstanceSetOmInstantKeyValueRes()
        {
            StageId = 0;
        }

        public S2CInstanceSetOmInstantKeyValueRes(C2SInstanceSetOmInstantKeyValueReq reqData)
        {
            ReqData = reqData;
        }

        public uint StageId { get; set; }
        public C2SInstanceSetOmInstantKeyValueReq ReqData { get; set; }
        

        public class Serializer : PacketEntitySerializer<S2CInstanceSetOmInstantKeyValueRes>
        {
            public override void Write(IBuffer buffer, S2CInstanceSetOmInstantKeyValueRes obj)
            {
                WriteUInt64(buffer, 0);
                C2SInstanceSetOmInstantKeyValueReq reqData = obj.ReqData;
                WriteUInt32(buffer, obj.StageId);
                WriteUInt64(buffer, reqData.Data0);
                WriteUInt32(buffer, reqData.Data1);
                WriteByteArray(buffer, obj.ResData);
            }

            public override S2CInstanceSetOmInstantKeyValueRes Read(IBuffer buffer)
            {
                S2CInstanceSetOmInstantKeyValueRes obj = new S2CInstanceSetOmInstantKeyValueRes();
                return obj;
            }
        }

        private readonly byte[] ResData = { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };

    }
}
