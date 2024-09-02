using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceSetOmInstantKeyValueRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_INSTANCE_SET_OM_INSTANT_KEY_VALUE_RES;
        
        public S2CInstanceSetOmInstantKeyValueRes()
        {
            StageId = 0;
        }

        public uint StageId { get; set; }
        public ulong Key {  get; set; }
        public uint Value {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CInstanceSetOmInstantKeyValueRes>
        {
            public override void Write(IBuffer buffer, S2CInstanceSetOmInstantKeyValueRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.StageId);
                WriteUInt64(buffer, obj.Key);
                WriteUInt32(buffer, obj.Value);
            }

            public override S2CInstanceSetOmInstantKeyValueRes Read(IBuffer buffer)
            {
                S2CInstanceSetOmInstantKeyValueRes obj = new S2CInstanceSetOmInstantKeyValueRes();
                ReadServerResponse(buffer, obj);
                obj.StageId = ReadUInt32(buffer);
                obj.Key = ReadUInt64(buffer);
                obj.Value = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
