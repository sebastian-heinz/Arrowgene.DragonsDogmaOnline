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
            
            // header ??
            test.WriteUInt16(2); 
            test.WriteUInt16(0); 
           // test.WriteInt16(0); 
            // header ??
            
            // respo struc
            test.WriteUInt16(1); //us_error
            test.WriteUInt32(1); //n_result
            test.WriteByte(16);  //uc_PasswordSrcSize
            test.WriteByte(16);  //ucPasswordENcSize
            test.WriteCString("abcdef0123456789"); // max 0x3d - 0terminated bytes @0x12
            
            byte[] enc = client.DdoNetworkCrypto.Encrypt(test.GetAllBytes());

            client.Send(new Packet(enc));
        }
    };
}
