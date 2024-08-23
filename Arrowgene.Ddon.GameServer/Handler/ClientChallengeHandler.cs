using Arrowgene.Buffers;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClientChallengeHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClientChallengeHandler));


        public ClientChallengeHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_CERT_CLIENT_CHALLENGE_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            client.SetChallengeCompleted(true);
            
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
            client.Send(new Packet(PacketId.S2C_CERT_CLIENT_CHALLENGE_RES, buffer.GetAllBytes()));
        }
    }
}
