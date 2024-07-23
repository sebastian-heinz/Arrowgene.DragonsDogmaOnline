using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ConnectionMoveInServerHandler : GameStructurePacketHandler<C2SConnectionMoveInServerReq>
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
            client.Character.Equipment = client.Character.Storage.GetCharacterEquipment();
            client.Character.Server = Server.AssetRepository.ServerList[0];
            client.UpdateIdentity();

            client.Character.Pawns = Database.SelectPawnsByCharacterId(token.CharacterId);
            for (int i = 0; i < client.Character.Pawns.Count; i++)
            {
                Pawn pawn = client.Character.Pawns[i];
                pawn.Server = client.Character.Server;
                pawn.Equipment = client.Character.Storage.GetPawnEquipment(i);
            }

            Logger.Info(client, "Moved Into GameServer");


            // NTC
            client.Send(new S2CItemExtendItemSlotNtc());
            // client.Send(GameFull.Dump_5);
            //  client.Send(GameFull.Dump_6);

            client.Send(new S2CConnectionMoveInServerRes());
        }
    }
}
