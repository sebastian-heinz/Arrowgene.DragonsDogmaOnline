using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyInvitePrepareAcceptHandler : StructurePacketHandler<GameClient, C2SPartyPartyInvitePrepareAcceptReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyInvitePrepareAcceptHandler));

        public PartyPartyInvitePrepareAcceptHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartyPartyInvitePrepareAcceptReq> packet)
        {
            client.Send(new S2CPartyPartyInvitePrepareAcceptRes());

            S2CPartyPartyInvitePrepareAcceptNtc ntc = new S2CPartyPartyInvitePrepareAcceptNtc();
            foreach(GameClient partyClient in client.PendingInvitedParty.Members)
            {
                partyClient.Send(ntc);
            }
            
            client.PendingInvitedParty.Members.Add(client);
            client.Party = client.PendingInvitedParty;
            client.PendingInvitedParty = null;
        }
    }
}