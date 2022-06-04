using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceExchangeOmInstantKeyValueRes : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_INSTANCE_EXCHANGE_OM_INSTANT_KEY_VALUE_RES;
        
        public S2CInstanceExchangeOmInstantKeyValueRes()
        {
            StageId = 0;
            Data0 = 0;
            Data1 = 0;
        }

        public uint StageId { get; set; }
        public ulong Data0 { get; set; }
        public uint Data1 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CInstanceExchangeOmInstantKeyValueRes>
        {
            public override void Write(IBuffer buffer, S2CInstanceExchangeOmInstantKeyValueRes obj)
            {
                WriteUInt64(buffer, 0);
                WriteUInt32(buffer, obj.StageId);
                WriteUInt64(buffer, obj.Data0);
                WriteUInt32(buffer, obj.Data1);
                WriteByteArray(buffer, obj.ResData);
            }

            public override S2CInstanceExchangeOmInstantKeyValueRes Read(IBuffer buffer)
            {
                S2CInstanceExchangeOmInstantKeyValueRes obj = new S2CInstanceExchangeOmInstantKeyValueRes();
                return obj;
            }
        }

        private readonly byte[] ResData = { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };

    }
}
