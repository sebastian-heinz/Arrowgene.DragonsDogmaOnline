using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Channels;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Rpc;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientLoginHandler : LoginStructurePacketHandler<C2LLoginReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClientLoginHandler));

        private readonly LoginServerSetting _setting;
        private readonly object _tokensInFLightLock;
        private readonly HashSet<string> _tokensInFlight;
        private readonly HttpClient _httpClient = new HttpClient();

        public ClientLoginHandler(DdonLoginServer server) : base(server)
        {
            _setting = server.Setting;
            _tokensInFLightLock = new object();
            _tokensInFlight = new HashSet<string>();

            string authToken = server.AssetRepository.ServerList.Find(x => x.Id == server.Id)?.RpcAuthToken ??
                throw new Exception($"Server with ID {server.Id} was not found in the ServerList asset.");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Internal", $"{server.Id}:{authToken}");
        }

        public override void Handle(LoginClient client, StructurePacket<C2LLoginReq> packet)
        {
            DateTime now = DateTime.UtcNow;
            client.SetChallengeCompleted(true);

            string oneTimeToken = packet.Structure.OneTimeToken;
            Logger.Debug(client, $"Received LoginToken:{oneTimeToken} for platform:{packet.Structure.PlatformType}");

            L2CLoginRes res = new L2CLoginRes();
            res.OneTimeToken = oneTimeToken;

            if (!LockToken(oneTimeToken))
            {
                Logger.Error(client, $"OneTimeToken {oneTimeToken} is in process.");
                res.Error = 1;
                client.Send(res);
                return;
            }

            try
            {
                Account account = Database.SelectAccountByLoginToken(oneTimeToken);
                if (_setting.AccountRequired)
                {
                    if (account == null)
                    {
                        Logger.Error(client, "Invalid OneTimeToken");
                        res.Error = 1;
                        client.Send(res);
                        return;
                    }
                }
                else
                {
                    // allow easy access
                    // assume token as account name, password & email
                    if (account == null)
                    {
                        account = Database.SelectAccountByName(oneTimeToken);
                        if (account == null)
                        {
                            account = Database.CreateAccount(oneTimeToken, oneTimeToken, oneTimeToken);
                            if (account == null)
                            {
                                Logger.Error(client,
                                    "Could not create account from OneTimeToken, choose another token");
                                res.Error = 2;
                                client.Send(res);
                                return;
                            }

                            Logger.Info(client, "Created new account from OneTimeToken");
                        }

                        account.LoginToken = oneTimeToken;
                        account.LoginTokenCreated = now;
                    }
                }

                if (!account.LoginTokenCreated.HasValue)
                {
                    Logger.Error(client, "No login token exists");
                    res.Error = 2;
                    client.Send(res);
                    return;
                }

                TimeSpan loginTokenAge = account.LoginTokenCreated.Value - now;
                if (loginTokenAge > TimeSpan.FromDays(7)) // TODO convert to setting
                {
                    Logger.Error(client, $"OneTimeToken Created at: {account.LoginTokenCreated} expired.");
                    res.Error = 1;
                    client.Send(res);
                    return;
                }

                List<Connection> connections = Database.SelectConnectionsByAccountId(account.Id);
                if (connections.Count > 0)
                {
                    Logger.Error(client, $"Already logged in");
                    res.Error = (uint) ErrorCode.ERROR_CODE_AUTH_MULTIPLE_LOGIN;
                    client.Send(res);

                    if (_setting.KickOnMultipleLogin)
                    {
                        foreach (var conn in connections)
                        {
                            RequestKick(conn);
                        }
                    }

                    return;
                }
                
                // Order Important,
                // account need to be only assigned after
                // verification that no connection exists, and before
                // registering the connection
                client.Account = account;
                
                Connection connection = new Connection();
                connection.ServerId = Server.Id;
                connection.AccountId = account.Id;
                connection.Type = ConnectionType.LoginServer;
                connection.Created = now;
                if (!Database.InsertConnection(connection))
                {
                    Logger.Error(client, $"Failed to register login connection");
                    res.Error = 1;
                    client.Send(res);
                    return;
                }

                client.Account.LastAuthentication = now;
                client.UpdateIdentity();
                Database.UpdateAccount(client.Account);

                Logger.Info(client, "Logged In");
                client.Send(res);
            }
            finally
            {
                ReleaseToken(oneTimeToken);
            }
        }

        private void ReleaseToken(string token)
        {
            lock (_tokensInFLightLock)
            {
                if (!_tokensInFlight.Contains(token))
                {
                    return;
                }

                _tokensInFlight.Remove(token);
            }
        }

        /// <summary>
        /// Locks a token, which can not be used in any other thread until released
        /// </summary>
        /// <param name="token"></param>
        /// <returns>true if token was locked, false if token already locked</returns>
        private bool LockToken(string token)
        {
            lock (_tokensInFLightLock)
            {
                if (_tokensInFlight.Contains(token))
                {
                    return false;
                }

                _tokensInFlight.Add(token);
                return true;
            }
        }

        private void RequestKick(Connection connection)
        {
            if (connection.Type == ConnectionType.LoginServer)
            {
                // Can't talk to the login server, but there's usually not a stuck connection here.
                return;
            }

            var channel = Server.AssetRepository.ServerList.Find(x => x.Id == connection.ServerId);
            var route = $"http://{channel.Addr}:{channel.RpcPort}/rpc/internal/command";

            var wrappedObject = new RpcWrappedObject()
            {
                Command = RpcInternalCommand.KickInternal,
                Origin = (ushort)Server.Id,
                Data = connection.AccountId
            };

            var json = JsonSerializer.Serialize(wrappedObject);
            _ = _httpClient.PostAsync(route, new StringContent(json));
        }
    }
}
