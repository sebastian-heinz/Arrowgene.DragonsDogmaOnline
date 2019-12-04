using Ddo.Server.Model;

namespace Ddo.Server.Packet
{
    public interface IClientHandler : IHandler
    {
        void Handle(DdoClient client, DdoPacket packet);
    }
}
