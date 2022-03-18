using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceSetOmInstantKeyValueRes : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_INSTANCE_SET_OM_INSTANT_KEY_VALUE_RES;
        
        public S2CInstanceSetOmInstantKeyValueRes()
        {
            Data0 = 0;
            Data1 = 0;
        }

        public S2CInstanceSetOmInstantKeyValueRes(C2SInstanceSetOmInstantKeyValueReq data01)
        {
            Data01 = data01;
        }

        public ulong Data0 { get; set; }
        public uint Data1 { get; set; }

        public C2SInstanceSetOmInstantKeyValueReq Data01 { get; set; }


        public class Serializer : PacketEntitySerializer<S2CInstanceSetOmInstantKeyValueRes>
        {
            public override void Write(IBuffer buffer, S2CInstanceSetOmInstantKeyValueRes obj)
            {
                WriteUInt64(buffer, 0);
                C2SInstanceSetOmInstantKeyValueReq reqData01 = obj.Data01;
                WriteUInt32(buffer, 0); //DummyStageId
                WriteUInt64(buffer, reqData01.Data0);
                WriteUInt32(buffer, reqData01.Data1);
                WriteByteArray(buffer, obj.Pad15);
            }

            public override S2CInstanceSetOmInstantKeyValueRes Read(IBuffer buffer)
            {
                S2CInstanceSetOmInstantKeyValueRes obj = new S2CInstanceSetOmInstantKeyValueRes();
                return obj;
            }
        }

        private readonly byte[] Pad15 = { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };

    }
}
