using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceExchangeOmInstantKeyValueRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_INSTANCE_EXCHANGE_OM_INSTANT_KEY_VALUE_RES;
        
        public S2CInstanceExchangeOmInstantKeyValueRes()
        {
            StageId = 0;
            Key = 0;
            Value = 0;
            OldValue = 0;
        }

        public uint StageId { get; set; }
        public ulong Key { get; set; }
        public uint Value { get; set; }
        public uint OldValue { get; set; }

        public class Serializer : PacketEntitySerializer<S2CInstanceExchangeOmInstantKeyValueRes>
        {
            public override void Write(IBuffer buffer, S2CInstanceExchangeOmInstantKeyValueRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.StageId);
                WriteUInt64(buffer, obj.Key);
                WriteUInt32(buffer, obj.Value);
                WriteUInt32(buffer, obj.OldValue);
            }

            public override S2CInstanceExchangeOmInstantKeyValueRes Read(IBuffer buffer)
            {
                S2CInstanceExchangeOmInstantKeyValueRes obj = new S2CInstanceExchangeOmInstantKeyValueRes();
                ReadServerResponse(buffer, obj);
                obj.StageId = ReadUInt32(buffer);
                obj.Key = ReadUInt64(buffer);
                obj.Value = ReadUInt32(buffer);
                obj.OldValue = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
