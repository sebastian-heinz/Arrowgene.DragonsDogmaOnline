using Ddo.Server.Model;

namespace Ddo.Server.Packet
{
    public abstract class ClientHandlerDeserializer<T> : ClientHandler
    {
        private readonly IPacketDeserializer<T> _deserializer;

        protected ClientHandlerDeserializer(DdoServer server, IPacketDeserializer<T> deserializer) : base(server)
        {
            _deserializer = deserializer;
        }

        public override void Handle(DdoClient client, DdoPacket requestPacket)
        {
            T request = _deserializer.Deserialize(requestPacket);
            HandleRequest(client, request);
        }

        public abstract void HandleRequest(DdoClient client, T request);
    }
}
