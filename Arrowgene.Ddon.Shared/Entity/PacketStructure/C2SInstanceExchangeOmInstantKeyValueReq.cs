using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceExchangeOmInstantKeyValueReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_EXCHANGE_OM_INSTANT_KEY_VALUE_REQ;

        public ulong Data0 { get; set; }
        public uint Data1 { get; set; }
        
        public C2SInstanceExchangeOmInstantKeyValueReq()
        {
            Data0 = 0;
            Data1 = 0;
        }
        
        public class Serializer : PacketEntitySerializer<C2SInstanceExchangeOmInstantKeyValueReq>
        {
            public override void Write(IBuffer buffer, C2SInstanceExchangeOmInstantKeyValueReq obj)
            {
                WriteUInt64(buffer, obj.Data0);
                WriteUInt32(buffer, obj.Data1);
            }

            public override C2SInstanceExchangeOmInstantKeyValueReq Read(IBuffer buffer)
            {
                C2SInstanceExchangeOmInstantKeyValueReq obj = new C2SInstanceExchangeOmInstantKeyValueReq();
                obj.Data0 = ReadUInt64(buffer);
                obj.Data1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}


