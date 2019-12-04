using Ddo.Server.Model;

namespace Ddo.Server.Packet
{
    public abstract class ConnectionHandler : Handler, IConnectionHandler
    {
        protected ConnectionHandler(DdoServer server) : base(server)
        {
        }
        
        public abstract void Handle(DdoConnection client, DdoPacket packet);
    }
}
