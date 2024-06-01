using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBazaarReceiveProceedsRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BAZAAR_RECEIVE_PROCEEDS_RES;

        public uint Proceeds { get; set; }

        public class Serializer : PacketEntitySerializer<S2CBazaarReceiveProceedsRes>
        {
            public override void Write(IBuffer buffer, S2CBazaarReceiveProceedsRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.Proceeds);
            }

            public override S2CBazaarReceiveProceedsRes Read(IBuffer buffer)
            {
                S2CBazaarReceiveProceedsRes obj = new S2CBazaarReceiveProceedsRes();
                ReadServerResponse(buffer, obj);
                obj.Proceeds = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}