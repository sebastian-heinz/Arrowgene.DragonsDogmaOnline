using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
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
            Party newParty = client.PendingInvitedParty; // In case some other thread changes the value
            client.PendingInvitedParty = null;

            client.Send(new S2CPartyPartyInvitePrepareAcceptRes());

            S2CPartyPartyInviteAcceptNtc inviteAcceptNtc = new S2CPartyPartyInviteAcceptNtc();
            inviteAcceptNtc.ServerId = 0; // TODO: Get from config, or from DdonGameServer instance
            inviteAcceptNtc.PartyId = newParty.Id;
            inviteAcceptNtc.StageId = newParty.Leader.Stage.Id;
            inviteAcceptNtc.PositionId = 0; // TODO: Figure what this is about
            inviteAcceptNtc.MemberIndex = (byte) (newParty.Members.Count + 1); // 0 based? 1 based?
            client.Send(inviteAcceptNtc);

        }
    }
}