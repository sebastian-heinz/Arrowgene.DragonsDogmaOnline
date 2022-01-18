using System;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
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

        public override PacketId Id => PacketId.C2L_LOGIN_REQ;

        public override void Handle(LoginClient client, StructurePacket<C2LLoginReq> packet)
        {
            Logger.Debug(client, $"Received LoginToken:{packet.Structure.LoginToken} for platform:{packet.Structure.PlatformType}");

            L2CLoginRes res = new L2CLoginRes();
            res.LoginToken = packet.Structure.LoginToken;

            Account account = Database.SelectAccountByLoginToken(packet.Structure.LoginToken);
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
                    account = Database.CreateAccount(packet.Structure.LoginToken, packet.Structure.LoginToken, packet.Structure.LoginToken);
                    if (account == null)
                    {
                        Logger.Error(client, "Could not create account from LoginToken, choose another token");
                        res.Error = 1;
                        client.Send(res);
                        return;
                    }

                    // set and do not clear token
                    account.LoginToken = packet.Structure.LoginToken;
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
