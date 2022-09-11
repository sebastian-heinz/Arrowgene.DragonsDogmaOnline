using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ddon.GameServer
{
    public class GameClient : Client, IPartyMember
    {
        public GameClient(ITcpSocket socket, PacketFactory packetFactory) : base(socket, packetFactory)
        {
            UpdateIdentity();
        }

        public void UpdateIdentity()
        {
            string newIdentity = $"[GameClient@{Socket.Identity}]";
            if (Account != null)
            {
                newIdentity += $"[Acc:({Account.Id}){Account.NormalName}]";
            }

            if (Character != null)
            {
                newIdentity += $"[Cha:({Character.Id}){Character.FirstName} {Character.LastName}]";
            }

            Identity = newIdentity;
        }

        public Account Account { get; set; }

        public Character Character { get; set; }

        public Party PendingInvitedParty { get; set; } // Maybe its more clean to store this in the handlers ?
        public Party Party { get; set; }

    }
}
