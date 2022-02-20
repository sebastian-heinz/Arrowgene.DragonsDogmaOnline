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

        public override void Handle(LoginClient client, StructurePacket<C2LLoginReq> packet)
        {
            Logger.Debug(client, $"Received LoginToken:{packet.Structure.OneTimeToken} for platform:{packet.Structure.PlatformType}");

            L2CLoginRes res = new L2CLoginRes();
            res.OneTimeToken = packet.Structure.OneTimeToken;

            Account account = Database.SelectAccountByLoginToken(packet.Structure.OneTimeToken);
            if (_setting.AccountRequired)
            {
                if (account == null)
                {
                    Logger.Error(client, "Invalid OneTimeToken");
                    res.Error = 1;
                    client.Send(res);
                    return;
                }

                TimeSpan loginTokenAge = account.LoginTokenCreated - DateTime.Now;
                if (loginTokenAge > TimeSpan.FromDays(1)) // TODO convert to setting
                {
                    Logger.Error(client, $"OneTimeToken Created at: {account.LoginTokenCreated} expired.");
                    res.Error = 1;
                    client.Send(res);
                    return;
                }
            }
            else
            {
                // allow easy access
                // assume token as account name & password
                if (account == null)
                {
                    account = Database.SelectAccountByName(packet.Structure.OneTimeToken);
                    if (account == null)
                    {
                        account = Database.CreateAccount(packet.Structure.OneTimeToken, packet.Structure.OneTimeToken, packet.Structure.OneTimeToken);
                        if (account == null)
                        {
                            Logger.Error(client, "Could not create account from OneTimeToken, choose another token");
                            res.Error = 2;
                            client.Send(res);
                            return;
                        }

                        Logger.Info(client, "Created new account from OneTimeToken");
                    }

                    account.LoginToken = packet.Structure.OneTimeToken;
                    account.LoginTokenCreated = DateTime.Now;
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
