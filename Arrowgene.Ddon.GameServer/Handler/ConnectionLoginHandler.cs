using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ConnectionLoginHandler : GameRequestPacketHandler<C2SConnectionLoginReq, S2CConnectionLoginRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ConnectionLoginHandler));

        public ConnectionLoginHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CConnectionLoginRes Handle(GameClient client, C2SConnectionLoginReq request)
        {
            client.SetChallengeCompleted(true);

            Logger.Debug(client,
                $"Received SessionKey:{request.SessionKey} for platform:{request.PlatformType}");

            S2CConnectionLoginRes res = new S2CConnectionLoginRes();
            GameToken token = Database.SelectToken(request.SessionKey);
            if (token == null)
            {
                Logger.Error(client, $"SessionKey:{request.SessionKey} not found");
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_COG_ANALIZE_SESSION_KEY);
            }

            if (!Database.DeleteTokenByAccountId(token.AccountId))
            {
                Logger.Error(client, $"Failed to delete session key from DB:{request.SessionKey}");
            }


            Account account = Database.SelectAccountById(token.AccountId);
            if (account == null)
            {
                Logger.Error(client, $"AccountId:{token.AccountId} not found");
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_FAIL);
            }

            DateTime now = DateTime.UtcNow;

            List<Connection> connections = Database.SelectConnectionsByAccountId(account.Id);
            if (connections.Count > 0)
            {
                foreach (Connection con in connections)
                {
                    if (con.Type == ConnectionType.GameServer)
                    {
                        Logger.Error(client, $"game server connection already exists");
                        throw new ResponseErrorException(ErrorCode.ERROR_CODE_AUTH_DUPLICATE_DATA);
                    }
                }
            }

            // Order Important,
            // account need to be only assigned after
            // verification that no connection exists, and before
            // registering the connection
            client.Account = account;

            Connection connection = new Connection();
            connection.ServerId = Server.Id;
            connection.AccountId = account.Id;
            connection.Type = ConnectionType.GameServer;
            connection.Created = now;
            if (!Database.InsertConnection(connection))
            {
                Logger.Error(client, $"Failed to register game connection");
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_NET_NOT_CONNECT_GAME_SERVER);
            }

            Character character = Server.CharacterManager.SelectCharacter(client, token.CharacterId);
            if (character == null)
            {
                Logger.Error(client, $"CharacterId:{token.CharacterId} not found");
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_CHARACTER_DATA_INVALID_CHARACTER_ID);
            }

            Logger.Info(client, "Logged Into GameServer");

            // update login token for client
            // client.Account.LoginToken = GameToken.GenerateLoginToken();
            client.Account.LoginTokenCreated = now;
            if (!Database.UpdateAccount(client.Account))
            {
                Logger.Error(client, "Failed to update OneTimeToken");
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_AUTH_SESSION_KEY_GENERATE);
            }

            Server.RpcManager.AnnouncePlayerList();

            Logger.Debug(client, $"Updated OneTimeToken:{client.Account.LoginToken}");

            res.OneTimeToken = client.Account.LoginToken;

            return res;
        }
    }
}
