using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyCreateHandler : GameStructurePacketHandler<C2SPartyPartyCreateReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyCreateHandler));

        public PartyPartyCreateHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartyPartyCreateReq> packet)
        {
            PartyGroup party = Server.PartyManager.NewParty();
            if (party == null)
            {
                Logger.Error(client, "Failed to create party");
                // TODO return error
                return;
            }

            PlayerPartyMember member = party.Invite(client);
            if (member == null)
            {
                Logger.Error(client, "Failed to join new party");
                // TODO return error
                return;
            }

            member = party.Accept(client);
            if (member == null)
            {
                Logger.Error(client, "Failed to accept new party");
                // TODO return error
                return;
            }

            member = party.Join(client);
            if (member == null)
            {
                Logger.Error(client, "Failed to join new party");
                // TODO return error
                return;
            }

            Logger.Info(client, $"Created Party with PartyId:{party.Id}");

            S2CPartyPartyJoinNtc partyJoinNtc = new S2CPartyPartyJoinNtc();
            partyJoinNtc.HostCharacterId = client.Character.Id;
            partyJoinNtc.LeaderCharacterId = client.Character.Id;
            partyJoinNtc.PartyMembers.Add(member.GetCDataPartyMember());
            client.Send(partyJoinNtc);
            
            S2CPartyPartyCreateRes partyCreateRes = new S2CPartyPartyCreateRes();
            partyCreateRes.PartyId = party.Id;
            client.Send(partyCreateRes);

        }
    }
}
