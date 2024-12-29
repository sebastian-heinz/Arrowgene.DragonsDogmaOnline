using Arrowgene.Buffers;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientChallengeHandler : LoginRequestPacketHandler<C2LClientChallengeReq, L2CClientChallengeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClientChallengeHandler));


        public ClientChallengeHandler(DdonLoginServer server) : base(server)
        {
        }

        public override L2CClientChallengeRes Handle(LoginClient client, C2LClientChallengeReq request)
        {
            Challenge.Response challenge = client.HandleChallenge(request);
            if (challenge.Error)
            {
                Logger.Error(client, "Failed CertChallenge");
                throw new ResponseErrorException(Shared.Model.ErrorCode.ERROR_CODE_AUTH_LOGIN_FAILED);
            }

            return new L2CClientChallengeRes()
            {
                PasswordSrcSize = challenge.DecryptedBlowFishKeyLength,
                PasswordEncSize = challenge.EncryptedBlowFishKeyLength,
                PasswordEnc = challenge.EncryptedBlowFishPassword
            };
        }
    }
}
