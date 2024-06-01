using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SInstanceSetOmInstantKeyValueReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_INSTANCE_SET_OM_INSTANT_KEY_VALUE_REQ;

        public ulong Key { get; set; }
        public uint Value { get; set; }
        
        public C2SInstanceSetOmInstantKeyValueReq()
        {
            Key = 0;
            Value = 0;
        }
        
        public class Serializer : PacketEntitySerializer<C2SInstanceSetOmInstantKeyValueReq>
        {
            public override void Write(IBuffer buffer, C2SInstanceSetOmInstantKeyValueReq obj)
            {
                WriteUInt64(buffer, obj.Key);
                WriteUInt32(buffer, obj.Value);
            }

            public override C2SInstanceSetOmInstantKeyValueReq Read(IBuffer buffer)
            {
                C2SInstanceSetOmInstantKeyValueReq obj = new C2SInstanceSetOmInstantKeyValueReq();
                obj.Key = ReadUInt64(buffer);
                obj.Value = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
