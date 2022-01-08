using Arrowgene.Buffers;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientChallengeHandler : PacketHandler<LoginClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ClientChallengeHandler));


        public ClientChallengeHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_CLIENT_CHALLENGE_REQ;

        public override void Handle(LoginClient client, Packet packet)
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
            client.Send(new Packet(PacketId.L2C_CLIENT_CHALLENGE_RES, buffer.GetAllBytes(), PacketSource.Server));
        }
    }
}
