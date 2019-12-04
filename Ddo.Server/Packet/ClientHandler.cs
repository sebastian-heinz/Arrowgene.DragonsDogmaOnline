using Ddo.Server.Model;

namespace Ddo.Server.Packet
{
    public abstract class ClientHandler : Handler, IClientHandler
    {
        protected ClientHandler(DdoServer server) : base(server)
        {
        }

        public abstract void Handle(DdoClient client, DdoPacket packet);
    }
}
