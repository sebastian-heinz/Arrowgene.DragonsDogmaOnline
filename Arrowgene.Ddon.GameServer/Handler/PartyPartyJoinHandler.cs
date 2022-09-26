using System.Linq;
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
                Logger.Error(client, "Could not join party");
                // todo return error
                return;
            }

            party.Members.Add(client);
            client.PendingInvitedParty = null;
            client.Party = party;

            S2CPartyPartyJoinRes response = new S2CPartyPartyJoinRes();
            response.PartyId = party.Id;
            client.Send(response);

            // Send updated member list to all party members
            S2CPartyPartyJoinNtc partyJoinNtc = new S2CPartyPartyJoinNtc();
            partyJoinNtc.HostCharacterId = party.Host.Character.Id;
            partyJoinNtc.LeaderCharacterId = party.Leader.Character.Id;
            partyJoinNtc.PartyMembers = party.Members
                .Select(x => x.AsCDataPartyMember())
                .ToList();
            party.SendToAll(partyJoinNtc);

            // Send party player context NTCs to the new member
            for(byte i = 0; i < party.Members.Count; i++)
            {
                GameClient member = party.Members[i];
                party.SendToAll(member.AsContextPacket());
            }
        }
    }
}
