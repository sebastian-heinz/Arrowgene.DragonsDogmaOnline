using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyLeaveHandler : GameStructurePacketHandler<C2SPartyPartyLeaveReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyLeaveHandler));

        public PartyPartyLeaveHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartyPartyLeaveReq> packet)
        {
            PartyGroup party = client.Party;

            if (party == null)
            {
                Logger.Error(client, "Could not leave party, does not exist");
                // todo return error
                return;
            }

            S2CPartyPartyLeaveNtc partyLeaveNtc = new S2CPartyPartyLeaveNtc();
            partyLeaveNtc.CharacterId = client.Character.Id;
            party.SendToAll(partyLeaveNtc);

            party.Leave(client);
            Logger.Info(client, $"Left PartyId:{party.Id}");

            client.Send(new S2CPartyPartyLeaveRes());
        }
    }
}
