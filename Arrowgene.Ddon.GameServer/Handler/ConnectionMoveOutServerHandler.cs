using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ConnectionMoveOutServerHandler : StructurePacketHandler<GameClient, C2SConnectionMoveOutServerReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ConnectionMoveOutServerHandler));

        public ConnectionMoveOutServerHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SConnectionMoveOutServerReq> packet)
        {
            Logger.Debug(client, $"Creating SessionKey");
            S2CConnectionMoveOutServerRes res = new S2CConnectionMoveOutServerRes();
            GameToken token = GameToken.GenerateGameToken(client.Account.Id, client.Character.CharacterId);
            if (!Database.SetToken(token))
            {
                Logger.Error(client, "Failed to store SessionKey");
                res.Error = (uint)ErrorCode.ERROR_CODE_AUTH_SESSION_KEY_GENERATE;
                client.Send(res);
                return;
            }

            Logger.Info(client, $"Created SessionKey:{token.Token} for CharacterId:{client.Character.CharacterId}");
            res.SessionKey = token.Token;
            client.Send(res);
        }
    }
}
