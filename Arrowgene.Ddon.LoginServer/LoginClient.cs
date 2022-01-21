using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ddon.LoginServer
{
    public class LoginClient : Client
    {
        public LoginClient(ITcpSocket socket, PacketFactory packetFactory) : base(socket, packetFactory)
        {
            UpdateIdentity();
        }

        public void UpdateIdentity()
        {
            string newIdentity = $"[LoginClient@{Socket.Identity}]";
            if (Account != null)
            {
                newIdentity += $"[Acc:{Account.NormalName}]";
            }

            Identity = newIdentity;
        }

        public Account Account { get; set; }
        
        public uint SelectedCharacterId { get; set; }
    }
}
