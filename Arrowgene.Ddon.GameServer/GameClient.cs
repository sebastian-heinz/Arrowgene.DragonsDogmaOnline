using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ddon.GameServer
{
    public class GameClient : Client
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
                newIdentity += $"[Cha:({Character.Id}){Character.FirstName}]";
            }

            Identity = newIdentity;
        }

        public Account Account { get; set; }

        public Character Character { get; set; }

        /// TODO combine into a location class ?
        public StageId Stage { get; set; }
        public uint StageNo { get; set; }
        public double X { get; set; }
        public float Y { get; set; }
        public double Z { get; set; }
        // ---
    }
}
