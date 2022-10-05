using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
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
            S2CPartyPartyCreateRes partyCreateRes = new S2CPartyPartyCreateRes();

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

            ErrorRes<PlayerPartyMember> memberRes = party.Accept(client);
            if (memberRes.HasError)
            {
                Logger.Error(client, "Failed to accept new party");
                partyCreateRes.Error = (uint)memberRes.ErrorCode;
                client.Send(partyCreateRes);
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
            
        
            partyCreateRes.PartyId = party.Id;
            client.Send(partyCreateRes);

        }
    }
}
