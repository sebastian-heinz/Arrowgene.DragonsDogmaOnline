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

        public override PacketId Id => PacketId.C2L_CLIENT_CHALLENGE_REQ;

        public override void Handle(Client client, Packet packet)
        {
            Challenge.Response challenge = client.HandleChallenge(packet.Data);
            if (challenge.Error)
            {
                Logger.Error(client, "Failed CertChallenge");
                return;
            }

            IBuffer buffer = new StreamBuffer();
            buffer.WriteInt32(0); //us_error
            buffer.WriteInt32(0); //n_result
            buffer.WriteByte(challenge.DecryptedBlowFishKeyLength); //uc_PasswordSrcSize
            buffer.WriteByte(challenge.EncryptedBlowFishKeyLength); //ucPasswordENcSize
            buffer.WriteBytes(challenge.EncryptedBlowFishPassword);
            buffer.WriteBytes(new byte[48]);
            client.Send(new Packet(PacketId.L2C_CLIENT_CHALLENGE_RES, buffer.GetAllBytes()));
        }
    }
}
