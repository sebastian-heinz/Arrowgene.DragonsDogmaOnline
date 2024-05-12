using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceExchangeOmInstantKeyValueReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_EXCHANGE_OM_INSTANT_KEY_VALUE_REQ;

        public ulong Key { get; set; }
        public uint Value { get; set; }
        
        public C2SInstanceExchangeOmInstantKeyValueReq()
        {
            Key = 0;
            Value = 0;
        }
        
        public class Serializer : PacketEntitySerializer<C2SInstanceExchangeOmInstantKeyValueReq>
        {
            public override void Write(IBuffer buffer, C2SInstanceExchangeOmInstantKeyValueReq obj)
            {
                WriteUInt64(buffer, obj.Key);
                WriteUInt32(buffer, obj.Value);
            }

            public override C2SInstanceExchangeOmInstantKeyValueReq Read(IBuffer buffer)
            {
                C2SInstanceExchangeOmInstantKeyValueReq obj = new C2SInstanceExchangeOmInstantKeyValueReq();
                obj.Key = ReadUInt64(buffer);
                obj.Value = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}


