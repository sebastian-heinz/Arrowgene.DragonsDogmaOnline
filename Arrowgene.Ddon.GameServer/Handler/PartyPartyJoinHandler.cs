using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyJoinHandler : StructurePacketHandler<GameClient, C2SPartyPartyJoinReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyJoinHandler));

        public PartyPartyJoinHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartyPartyJoinReq> packet)
        {
            // TODO: Optimize
            Party newParty = ((DdonGameServer) Server).Parties.Find(x => x.Id == packet.Structure.PartyId);

            newParty.Members.Add(client);
            client.PendingInvitedParty = null;
            client.Party = newParty;

            S2CPartyPartyJoinRes response = new S2CPartyPartyJoinRes();
            response.PartyId = newParty.Id;
            client.Send(response);

            // Send updated member list to all party members
            S2CPartyPartyJoinNtc partyJoinNtc = new S2CPartyPartyJoinNtc();
            partyJoinNtc.HostCharacterId = newParty.Host.Character.Id;
            partyJoinNtc.LeaderCharacterId = newParty.Leader.Character.Id;
            partyJoinNtc.PartyMembers = newParty.Members
                .Select(x => x.AsCDataPartyMember())
                .ToList();
            newParty.SendToAll(partyJoinNtc);

            // Send party player context NTCs to the new member
            for(byte i = 0; i < newParty.Members.Count; i++)
            {
                IPartyMember member = newParty.Members[i];
                newParty.SendToAll(member.AsContextPacket());
            }
        }
    }
}