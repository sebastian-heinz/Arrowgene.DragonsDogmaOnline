using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
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

        public ClientLoginHandler(DdonLoginServer server) : base(server)
        {
            _setting = server.Setting;
            _tokensInFLightLock = new object();
            _tokensInFlight = new HashSet<string>();
        }

        public override void Handle(LoginClient client, StructurePacket<C2LLoginReq> packet)
        {
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
                        ReleaseToken(oneTimeToken);
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
                                ReleaseToken(oneTimeToken);
                                return;
                            }

                            Logger.Info(client, "Created new account from OneTimeToken");
                        }

                        account.LoginToken = oneTimeToken;
                        account.LoginTokenCreated = DateTime.Now;
                    }
                }

                if (account.LoggedIn)
                {
                    LoginClient loggedInClient = Server.ClientLookup.GetClientByAccountId(account.Id);
                    if (loggedInClient != null)
                    {
                        // see if the client has a active login connection and close it
                        loggedInClient.Close();
                    }

                    // TODO communicate to GS to close client
                    // since login and game server can run on different physical boxes
                    // this can only be done if a inter-server-communication system is in place.
                    // or it needs to be handled if the same client shows up again in GS
                    // for now, just deny another connection - this will ease finding bugs where
                    // the state of logged in is not properly updated.
                    Logger.Error(client, $"already logged in");
                    res.Error = 1;
                    client.Send(res);
                    ReleaseToken(oneTimeToken);
                    return;
                }

                TimeSpan loginTokenAge = account.LoginTokenCreated - DateTime.Now;
                if (loginTokenAge > TimeSpan.FromDays(1)) // TODO convert to setting
                {
                    Logger.Error(client, $"OneTimeToken Created at: {account.LoginTokenCreated} expired.");
                    res.Error = 1;
                    client.Send(res);
                    ReleaseToken(oneTimeToken);
                    return;
                }

                account.LastLogin = DateTime.Now;
                account.LoggedIn = true;
                Database.UpdateAccount(account);

                client.Account = account;
                client.UpdateIdentity();
                Logger.Info(client, "Logged In");

                client.Send(res);
            }
            finally
            {
                // in case of a exception, ensure our token is not locked up forever
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
    }
}
