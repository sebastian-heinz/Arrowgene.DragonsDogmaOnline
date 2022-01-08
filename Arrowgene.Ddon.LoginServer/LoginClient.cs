using Arrowgene.Ddon.Server.Network;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ddon.LoginServer
{
    public class LoginClient : Client
    {
        public LoginClient(ITcpSocket socket, PacketFactory packetFactory) : base(socket, packetFactory)
        {
        }

        public string SessionKey { get; set; }
    }
}
