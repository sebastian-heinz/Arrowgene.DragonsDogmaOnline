using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public sealed class ClientGetGameSessionKeyHandler(DdonLoginServer server) : LoginRequestPacketHandler<C2LGetGameSessionKeyReq, L2CGetGameSessionKeyRes>(server)
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClientGetGameSessionKeyHandler));

        public override L2CGetGameSessionKeyRes Handle(LoginClient client, C2LGetGameSessionKeyReq request)
        {
            Logger.Debug(client, $"Creating SessionKey for CharacterId:{client.SelectedCharacterId}");

            GameToken token = GameToken.GenerateGameToken(client.Account.Id, client.SelectedCharacterId);
            if (!Database.ReplaceToken(token))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_AUTH_SESSION_KEY_GENERATE, "Failed to store GameToken");
            }
            Logger.Info(client, $"Created SessionKey:{token.Token} for CharacterId:{client.SelectedCharacterId}");
            
            L2CGetGameSessionKeyRes res = new()
            {
                SessionKey = token.Token,
                // TODO: combine with character decide handler logic somehow? This should refer to a game server ID
                GameServerUniqueID = (ushort)Server.Setting.ServerSetting.Id
            };
            return res;
        }
    }
}
