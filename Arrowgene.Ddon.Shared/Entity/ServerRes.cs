using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity
{
    public class ServerRes : ServerResponse
    {
        public ServerRes()
        {
            Id = PacketId.UNKNOWN;
        }

        public ServerRes(PacketId packetId)
        {
            Id = packetId;
        }

        public override PacketId Id { get; }

        public class Serializer : EntitySerializer<ServerRes>
        {
            public override void Write(IBuffer buffer, ServerRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override ServerRes Read(IBuffer buffer)
            {
                ServerRes obj = new ServerRes(PacketId.UNKNOWN);
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
