using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Rpc;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientLoginHandler : LoginStructurePacketHandler<C2LLoginReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClientLoginHandler));

        private readonly LoginServerSetting _setting;
        private readonly object _tokensInFLightLock;
        private readonly HashSet<string> _tokensInFlight;

        private readonly HttpClient _httpClient = new HttpClient();
        private bool _httpReady = false;

        public ClientLoginHandler(DdonLoginServer server) : base(server)
        {
            _setting = server.Setting;
            _tokensInFLightLock = new object();
            _tokensInFlight = new HashSet<string>();
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
                res.Error = (uint)ErrorCode.ERROR_CODE_AUTH_ONETIME_TOKEN_FAIL;
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
                        res.Error = (uint)ErrorCode.ERROR_CODE_AUTH_ONETIME_TOKEN_FAIL;
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
                    res.Error = (uint)ErrorCode.ERROR_CODE_AUTH_ONETIME_TOKEN_FAIL;
                    client.Send(res);
                    return;
                }

                List<Connection> connections = Database.SelectConnectionsByAccountId(account.Id);

                if (_setting.KickOnMultipleLogin)
                {
                    for (uint tryCount = 0; tryCount < _setting.KickOnMultipleLoginTries; tryCount++)
                    {
                        if (connections.Any())
                        {
                            connections.ForEach(x => RequestKick(x));
                            Thread.Sleep(_setting.KickOnMultipleLoginTimer);
                            connections = Database.SelectConnectionsByAccountId(account.Id);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (connections.Any())
                {
                    Logger.Error(client, $"Already logged in.");
                    res.Error = (uint)ErrorCode.ERROR_CODE_AUTH_MULTIPLE_LOGIN;
                    client.Send(res);
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
                    res.Error = (uint)ErrorCode.ERROR_CODE_AUTH_LOGIN_FAILED;
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
            // Timing issues with loading files vs server process startup.
            if (!_httpReady)
            {
                lock(_httpClient)
                {
                    ServerInfo serverInfo = Server.AssetRepository.ServerList.Find(x => x.LoginId == Server.Id);
                    if (serverInfo is null)
                    {
                        Logger.Error($"Login server with ID {Server.Id} was not found in the ServerList asset.");
                        return;
                    }

                    // The login server auths as though it was the game server.
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Internal", $"{serverInfo.Id}:{serverInfo.RpcAuthToken}");
                    _httpReady = true;
                }
            }

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
