using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientGetGameSessionKeyHandler : LoginRequestPacketHandler<C2LGetGameSessionKeyReq, L2CGetGameSessionKeyRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClientGetGameSessionKeyHandler));

        public ClientGetGameSessionKeyHandler(DdonLoginServer server) : base(server)
        {
        }

        public override L2CGetGameSessionKeyRes Handle(LoginClient client, C2LGetGameSessionKeyReq request)
        {
            Logger.Debug(client, $"Creating SessionKey for CharacterId:{client.SelectedCharacterId}");
            L2CGetGameSessionKeyRes res = new L2CGetGameSessionKeyRes();
            GameToken token = GameToken.GenerateGameToken(client.Account.Id, client.SelectedCharacterId);
            if (!Database.SetToken(token))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_AUTH_SESSION_KEY_GENERATE, "Failed to store GameToken");
            }

            Logger.Info(client, $"Created SessionKey:{token.Token} for CharacterId:{client.SelectedCharacterId}");
            res.SessionKey = token.Token;
            return res;
        }
    }
}
