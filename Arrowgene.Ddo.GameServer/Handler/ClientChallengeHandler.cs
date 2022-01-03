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
            // header
            test.WriteByte(1); // group id
            test.WriteUInt16(0);  // handler id
            test.WriteByte(2);  // handler sub id
            // struc

            //
          //  test.WriteByte(0);
          //  test.WriteByte(0);
          //  test.WriteByte(0);
          //  test.WriteByte(0);
          //  test.WriteByte(0);
          //  //
          //  test.WriteByte(0);
          //  test.WriteByte(0);
          //  test.WriteByte(0);
          //  test.WriteByte(0);

            
           test.WriteUInt16(0); //us_error
            test.WriteUInt32(1); //n_result
            test.WriteByte(16);  //uc_PasswordSrcSize
            test.WriteByte(16);  //ucPasswordENcSize
            test.WriteBytes(client.DdoNetworkCrypto.GetEncryptedBlowFishPassword());
            
            byte[] enc = client.DdoNetworkCrypto.Encrypt(test.GetAllBytes());
            client.Send(new Packet(enc));
        }
    };
}
