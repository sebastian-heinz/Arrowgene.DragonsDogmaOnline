using System;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.LoginServer.Handler
{
    public class ClientLoginHandler : StructurePacketHandler<LoginClient, C2LLoginReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClientLoginHandler));

        private readonly LoginServerSetting _setting;

        public ClientLoginHandler(DdonLoginServer server) : base(server)
        {
            _setting = server.Setting;
        }

        public override void Handle(LoginClient client, StructurePacket<C2LLoginReq> packet)
        {
            string loginToken = packet.Structure.OneTimeToken;

            L2CLoginRes res = new L2CLoginRes();
            res.OneTimeToken = loginToken;

            if (loginToken.Length != GameToken.LoginTokenLength)
            {
                // If game server disconnects client, the client might come back with a different token.
                // Need to investigate, perhaps game server did not send proper error response to
                // terminate the connection
                Logger.Error(client, $"Invalid Login Token: {loginToken}");
                res.Error = 1;
                client.Send(res);
                return;
            }

            Logger.Debug(client, $"Received LoginToken:{packet.Structure.OneTimeToken} for platform:{packet.Structure.PlatformType}");

            Account account = Database.SelectAccountByLoginToken(packet.Structure.OneTimeToken);
            if (_setting.AccountRequired)
            {
                if (account == null)
                {
                    Logger.Error(client, "Invalid Token");
                    res.Error = 1;
                    client.Send(res);
                    return;
                }

                // clear token
                account.LoginToken = string.Empty;
            }
            else
            {
                // allow easy access
                // assume token as account name & password
                if (account == null)
                {
                    account = Database.CreateAccount(packet.Structure.OneTimeToken, packet.Structure.OneTimeToken, packet.Structure.OneTimeToken);
                    if (account == null)
                    {
                        Logger.Error(client, "Could not create account from LoginToken, choose another token");
                        res.Error = 1;
                        client.Send(res);
                        return;
                    }

                    // set and do not clear token
                    account.LoginToken = packet.Structure.OneTimeToken;
                    account.LoginTokenCreated = DateTime.Now;
                    Logger.Info(client, "Created new account from token");
                }
            }

            account.LastLogin = DateTime.Now;
            Database.UpdateAccount(account);

            client.Account = account;
            client.UpdateIdentity();
            Logger.Info(client, "Logged In");

            client.Send(res);
        }
    }
}
