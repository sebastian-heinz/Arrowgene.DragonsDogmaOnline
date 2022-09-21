using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ConnectionLoginHandler : StructurePacketHandler<GameClient, C2SConnectionLoginReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ConnectionLoginHandler));


        public ConnectionLoginHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SConnectionLoginReq> packet)
        {
            client.SetChallengeCompleted(true);
            
            Logger.Debug(client, $"Received SessionKey:{packet.Structure.SessionKey} for platform:{packet.Structure.PlatformType}");

            S2CConnectionLoginRes res = new S2CConnectionLoginRes();
            GameToken token = Database.SelectToken(packet.Structure.SessionKey);
            if (token == null)
            {
                Logger.Error(client, $"SessionKey:{packet.Structure.SessionKey} not found");
                res.Error = 1;
                client.Send(res);
                return;
            }

            if (!Database.DeleteTokenByAccountId(token.AccountId))
            {
                Logger.Error(client, $"Failed to delete session key from DB:{packet.Structure.SessionKey}");
            }


            Account account = Database.SelectAccountById(token.AccountId);
            if (account == null)
            {
                Logger.Error(client, $"AccountId:{token.AccountId} not found");
                res.Error = 1;
                client.Send(res);
                return;
            }

            Character character = Database.SelectCharacter(token.CharacterId);
            if (character == null)
            {
                Logger.Error(client, $"CharacterId:{token.CharacterId} not found");
                res.Error = 1;
                client.Send(res);
                return;
            }

            client.Account = account;
            client.Character = character;
            client.UpdateIdentity();
            Logger.Info(client, "Logged Into GameServer");

            // update login token for client
            client.Account.LoginToken = GameToken.GenerateLoginToken();
            client.Account.LoginTokenCreated = DateTime.Now;
            if (!Database.UpdateAccount(client.Account))
            {
                Logger.Error(client, "Failed to update OneTimeToken");
                res.Error = 1;
                client.Send(res);
                return;
            }

            Logger.Debug(client, $"Updated OneTimeToken:{client.Account.LoginToken}");

            res.OneTimeToken = client.Account.LoginToken;
            client.Send(res);
        }
    }
}
