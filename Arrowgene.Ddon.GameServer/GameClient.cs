using Arrowgene.Ddon.Shared;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ddon.GameServer
{
    public class GameClient : Client
    {
        public GameClient(ITcpSocket socket, PacketFactory packetFactory) : base(socket, packetFactory)
        {
            Identity = $"[GameClient@{socket.Identity}]";
        }
    }
}
