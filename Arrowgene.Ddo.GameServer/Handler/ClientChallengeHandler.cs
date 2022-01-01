using Arrowgene.Buffers;
using Arrowgene.Ddo.GameServer.Logging;
using Arrowgene.Ddo.GameServer.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddo.GameServer.Handler
{
    public class ClientChallengeHandler : PacketHandler
    {
        private static readonly DdoLogger Logger = LogProvider.Logger<DdoLogger>(typeof(ClientChallengeHandler));


        public ClientChallengeHandler(DdoGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.ClientChallengeReq_C2L;

        public override void Handle(Client client, Packet packet)
        {
            if (!client.DdoNetworkCrypto.HandleClientCertChallenge(packet.Data))
            {
                Logger.Error(client, "Failed CertChallenge");
                return;
            }

            IBuffer test = new StreamBuffer();
            test.WriteBytes(new byte[]{0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE});
            test.WriteBytes(new byte[]{0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE, 0xEE});
            byte[] enc = client.DdoNetworkCrypto.Encrypt(test.GetAllBytes());
            
            IBuffer test2 = new StreamBuffer();
            test2.WriteBytes(new byte[] {0x00,0x00,0x00, 0x02});
            test2.WriteBytes(enc);
            
            client.Send(test2.GetAllBytes());
        }
    };
}
