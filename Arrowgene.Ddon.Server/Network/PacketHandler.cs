using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Server.Network
{
    public abstract class PacketHandler<TClient> : IPacketHandler<TClient> where TClient : Client
    {
        protected PacketHandler(DdonServer<TClient> server)
        {
            Server = server;
            Database = server.Database;
        }

        protected DdonServer<TClient> Server { get; }
        protected IDatabase Database { get; }

        public abstract PacketId Id { get; }
        public abstract void Handle(TClient client, IPacket packet);

        public virtual void Dispose() {}
    }
}
