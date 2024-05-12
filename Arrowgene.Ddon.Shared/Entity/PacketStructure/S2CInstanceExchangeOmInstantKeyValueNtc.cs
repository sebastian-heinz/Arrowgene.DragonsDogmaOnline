using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceExchangeOmInstantKeyValueNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_INSTANCE_EXCHANGE_OM_INSTANT_KEY_VALUE_NTC;

        public S2CInstanceExchangeOmInstantKeyValueNtc()
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

        public class Serializer : PacketEntitySerializer<S2CInstanceExchangeOmInstantKeyValueNtc>
        {
            public override void Write(IBuffer buffer, S2CInstanceExchangeOmInstantKeyValueNtc obj)
            {
                WriteUInt32(buffer, obj.StageId);
                WriteUInt64(buffer, obj.Key);
                WriteUInt32(buffer, obj.Value);
                WriteUInt32(buffer, obj.OldValue);
            }

            public override S2CInstanceExchangeOmInstantKeyValueNtc Read(IBuffer buffer)
            {
                S2CInstanceExchangeOmInstantKeyValueNtc obj = new S2CInstanceExchangeOmInstantKeyValueNtc();
                obj.StageId = ReadUInt32(buffer);
                obj.Key = ReadUInt64(buffer);
                obj.Value = ReadUInt32(buffer);
                obj.OldValue = ReadUInt32(buffer);
                return obj;
            }

        }

    }
}
