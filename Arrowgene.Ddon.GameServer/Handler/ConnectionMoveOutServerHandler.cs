using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ConnectionMoveOutServerHandler : GameRequestPacketHandler<C2SConnectionMoveOutServerReq, S2CConnectionMoveOutServerRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ConnectionMoveOutServerHandler));

        public ConnectionMoveOutServerHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CConnectionMoveOutServerRes Handle(GameClient client, C2SConnectionMoveOutServerReq request)
        {
            Logger.Debug(client, $"Creating SessionKey");
            S2CConnectionMoveOutServerRes res = new S2CConnectionMoveOutServerRes();
            GameToken token = GameToken.GenerateGameToken(client.Account.Id, client.Character.CharacterId);
            if (!Database.SetToken(token))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_AUTH_SESSION_KEY_GENERATE, "Failed to store SessionKey");
            }

            Logger.Info(client, $"Created SessionKey:{token.Token} for CharacterId:{client.Character.CharacterId}");
            res.SessionKey = token.Token;
            return res;
        }
    }
}
