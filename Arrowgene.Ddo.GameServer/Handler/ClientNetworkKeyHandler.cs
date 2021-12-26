using Arrowgene.Buffers;
using Arrowgene.Ddo.GameServer.Network;

namespace Arrowgene.Ddo.GameServer.Handler
{
    public class ClientNetworkKeyHandler : PacketHandler
    {
        public ClientNetworkKeyHandler(DdoGameServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) PacketId.ClientNetworkKey;

        public override void Handle(Client client, Packet packet)
        {
            byte[] input = packet.Data.GetAllBytes();
            byte[] output = client.Handshake.ValidateClientCertChallenge(input);
            Packet response = new Packet(new StreamBuffer(output));
            client.Send(response);
        }
    };
}
