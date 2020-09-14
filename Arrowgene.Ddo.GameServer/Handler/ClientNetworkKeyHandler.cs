using System;
using Arrowgene.Ddo.GameServer;
using Arrowgene.Ddo.GameServer.Network;
using Arrowgene.Ddo.Shared;

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
            Console.WriteLine(packet.Data.ToAsciiString(" "));
            Console.WriteLine(packet.Data.ToHexString(" "));
            
            string hexResponse1 =
                "0060" +
                "3b440b4e0e65f4d73322e9f37c0d73ad" +
                "b4b72750bc9e7a45d14bf59e1031576f" +
                "db9dce65b0ce1743c69ce4a1dafd8eb5" +
                "175f0ec9372ed50d7b59f68ce1b87a13" +
                "d4472c8478240b3c37dd2229d254337b" +
                "ede3f8f1f60a0d263634a994b6abbd97";

            byte[] response1 = Util.FromHexString(hexResponse1);
            
            client.Send(response1);
        }
    };
}
