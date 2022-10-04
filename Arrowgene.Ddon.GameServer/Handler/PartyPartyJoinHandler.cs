using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyJoinHandler : GameStructurePacketHandler<C2SPartyPartyJoinReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyJoinHandler));

        public PartyPartyJoinHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartyPartyJoinReq> packet)
        {
            PartyGroup party = Server.PartyManager.GetParty(packet.Structure.PartyId);
            if (party == null)
            {
                Logger.Error(client, "Could not join party, does not exist");
                // todo return error
                return;
            }

            PartyMember partyMember = party.Join(client);
            if (partyMember == null)
            {
                Logger.Error(client, "Could not join party, its full");
                // todo return error
                return;
            }
            
            Logger.Info(client, $"Joined PartyId:{party.Id}");


            S2CPartyPartyJoinRes response = new S2CPartyPartyJoinRes();
            response.PartyId = party.Id;
            client.Send(response);

            // Send updated member list to all party members
            S2CPartyPartyJoinNtc partyJoinNtc = new S2CPartyPartyJoinNtc();
            partyJoinNtc.HostCharacterId = party.Host.Character.Id;
            partyJoinNtc.LeaderCharacterId = party.Leader.Character.Id;
            // Send party player context NTCs to the new member
            foreach (PartyMember member in party.Members)
            {
                partyJoinNtc.PartyMembers.Add(member.GetCDataPartyMember());
            }

            party.SendToAll(partyJoinNtc);


            // Send party player context NTCs to the new member
            foreach (PartyMember member in party.Members)
            {
                party.SendToAll(member.GetS2CContextGetParty_ContextNtc());
                // TODO only new member or all ?
            }
        }
    }
}
