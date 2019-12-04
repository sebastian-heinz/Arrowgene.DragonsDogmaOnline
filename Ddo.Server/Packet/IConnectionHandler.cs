using Ddo.Server.Model;

namespace Ddo.Server.Packet
{
    public interface IConnectionHandler : IHandler
    {
        void Handle(DdoConnection connection, DdoPacket packet);
    }
}
