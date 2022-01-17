using Arrowgene.Ddon.Shared;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ddon.LoginServer
{
    public class LoginClient : Client
    {
        public LoginClient(ITcpSocket socket, PacketFactory packetFactory) : base(socket, packetFactory)
        {
            Identity = $"[LoginClient@{socket.Identity}]";
        }

        public string OnetimeToken { get; set; }
    }
}
