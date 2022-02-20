using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientGetGameSessionKeyHandler : PacketHandler<LoginClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClientGetGameSessionKeyHandler));


        public ClientGetGameSessionKeyHandler(DdonLoginServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2L_GET_GAME_SESSION_KEY_REQ;

        public override void Handle(LoginClient client, IPacket packet)
        {
            // Request packet C2L_GET_GAME_SESSION_KEY_REQ has no data aside from header,
            // the rest is just padding/alignment to 16-byte boundary.

            Logger.Debug(client, $"Creating SessionKey for CharacterId:{client.SelectedCharacterId}");
            L2CGetGameSessionKeyRes res = new L2CGetGameSessionKeyRes();
            GameToken token = GameToken.GenerateGameToken(client.Account.Id, client.SelectedCharacterId);
            if (!Database.SetToken(token))
            {
                Logger.Error(client, "Failed to store GameToken");
                res.Error = 1;
                client.Send(res);
                return;
            }

            Logger.Info(client, $"Created SessionKey:{token.Token} for CharacterId:{client.SelectedCharacterId}");
            res.SessionKey = token.Token;
            client.Send(res);
        }
    }
}
