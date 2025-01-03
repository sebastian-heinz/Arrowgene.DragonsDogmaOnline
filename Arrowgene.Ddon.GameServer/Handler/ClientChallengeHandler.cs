using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClientChallengeHandler : GameRequestPacketHandler<C2SCertClientChallengeReq, S2CCertClientChallengeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClientChallengeHandler));

        public ClientChallengeHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCertClientChallengeRes Handle(GameClient client, C2SCertClientChallengeReq request)
        {
            client.SetChallengeCompleted(true);

            Challenge.Response challenge = client.HandleChallenge(request);
            if (challenge.Error)
            {
                Logger.Error(client, "Failed CertChallenge");
                throw new ResponseErrorException(Shared.Model.ErrorCode.ERROR_CODE_SYSTEM_INTERNAL);
            }

            return new S2CCertClientChallengeRes()
            {
                PasswordSrcSize = challenge.DecryptedBlowFishKeyLength,
                PasswordEncSize = challenge.EncryptedBlowFishKeyLength,
                PasswordEnc = challenge.EncryptedBlowFishPassword
            };
        }
    }
}
