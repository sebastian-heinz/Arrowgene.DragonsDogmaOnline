using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler;

public sealed class ClientChallengeHandler(DdonLoginServer server) : LoginRequestPacketHandler<C2LClientChallengeReq, L2CClientChallengeRes>(server)
{
    private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClientChallengeHandler));

    public override L2CClientChallengeRes Handle(LoginClient client, C2LClientChallengeReq request)
    {
        Challenge.Response challenge = client.HandleChallenge(request);
        if (challenge.Error)
        {
            Logger.Error(client, "Failed client challenge.");
            throw new ResponseErrorException(ErrorCode.ERROR_CODE_AUTH_LOGIN_FAILED);
        }

        return new L2CClientChallengeRes
        {
            PasswordSrcSize = challenge.DecryptedBlowFishKeyLength,
            PasswordEncSize = challenge.EncryptedBlowFishKeyLength,
            PasswordEnc = challenge.EncryptedBlowFishPassword
        };
    }
}
