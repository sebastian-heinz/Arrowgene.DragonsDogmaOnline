using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceSetOmInstantKeyValueNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_INSTANCE_SET_OM_INSTANT_KEY_VALUE_NTC;

        public S2CInstanceSetOmInstantKeyValueNtc()
        {
        }
        public uint StageId { get; set; }
        public ulong Key { get; set; }
        public uint Value { get; set; }

        public class Serializer : PacketEntitySerializer<S2CInstanceSetOmInstantKeyValueNtc>
        {
            public override void Write(IBuffer buffer, S2CInstanceSetOmInstantKeyValueNtc obj)
            {
                WriteUInt32(buffer, obj.StageId);
                WriteUInt64(buffer, obj.Key);
                WriteUInt32(buffer, obj.Value);
            }

            public override S2CInstanceSetOmInstantKeyValueNtc Read(IBuffer buffer)
            {
                S2CInstanceSetOmInstantKeyValueNtc obj = new S2CInstanceSetOmInstantKeyValueNtc();
                obj.StageId = ReadUInt32(buffer);
                obj.Key = ReadUInt64(buffer);
                obj.Value = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
