using Ddo.Server.Packet;

namespace Ddo.Server.Model
{
    public interface ISender
    {
        void Send(DdoPacket packet);
    }
}
