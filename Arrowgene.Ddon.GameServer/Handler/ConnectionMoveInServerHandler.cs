using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ConnectionMoveInServerHandler : StructurePacketHandler<GameClient, C2SConnectionMoveInServerReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ConnectionMoveInServerHandler));


        public ConnectionMoveInServerHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SConnectionMoveInServerReq> packet)
        {
            Logger.Debug(client, $"Received SessionKey:{packet.Structure.SessionKey}");
            GameToken token = Database.SelectToken(packet.Structure.SessionKey);
            if (token == null)
            {
                Logger.Error(client, $"SessionKey:{packet.Structure.SessionKey} not found");
                // TODO reply error
                // return;
            }

            Database.DeleteTokenByAccountId(token.AccountId);

            Account account = Database.SelectAccountById(token.AccountId);
            if (account == null)
            {
                Logger.Error(client, $"AccountId:{token.AccountId} not found");
                // TODO reply error
                // return;
            }

            Character character = Database.SelectCharacter(token.CharacterId);
            if (character == null)
            {
                Logger.Error(client, $"CharacterId:{token.CharacterId} not found");
                // TODO reply error
                // return;
            }

            client.Account = account;
            client.Character = character;
            client.UpdateIdentity();

            Logger.Info(client, "Moved Into GameServer");


            // NTC
            client.Send(GameFull.Dump_4);
            // client.Send(GameFull.Dump_5);
            //  client.Send(GameFull.Dump_6);

            client.Send(GameFull.Dump_7);
        }
    }
}
