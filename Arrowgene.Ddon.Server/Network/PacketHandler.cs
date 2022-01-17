using Arrowgene.Ddon.Server;

namespace Arrowgene.Ddon.Shared
{
    public abstract class PacketHandler<TClient> : IPacketHandler<TClient> where TClient : Client
    {
        protected PacketHandler(DdonServer<TClient> server)
        {
            Server = server;
        }

        protected DdonServer<TClient> Server { get; }

        public abstract PacketId Id { get; }
        public abstract void Handle(TClient client, Packet packet);
    }
}
