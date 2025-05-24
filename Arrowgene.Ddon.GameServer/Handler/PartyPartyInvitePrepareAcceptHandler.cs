using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyInvitePrepareAcceptHandler : GameRequestPacketQueueHandler<C2SPartyPartyInvitePrepareAcceptReq, S2CPartyPartyInvitePrepareAcceptRes>
    {
        private static readonly ServerLogger Logger =
            LogProvider.Logger<ServerLogger>(typeof(PartyPartyInvitePrepareAcceptHandler));

        public PartyPartyInvitePrepareAcceptHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SPartyPartyInvitePrepareAcceptReq request)
        {
            PacketQueue queue = new();
            S2CPartyPartyInvitePrepareAcceptRes res = new S2CPartyPartyInvitePrepareAcceptRes();

            PartyInvitation invitation = Server.PartyManager.GetPartyInvitation(client)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_NOT_FOUNDED, "failed to find invitation");

            PartyGroup party = invitation.Party
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_NOT_FOUNDED, "failed to find inviting party");

            PlayerPartyMember partyMember = party.Accept(client);
            
            Logger.Info(client, $"Accepted Invite for PartyId:{party.Id}");
            client.Enqueue(res, queue);

            // The invited player doesn't move to the new party leader's server until this packet is sent
            // Why this wasn't included in the Response packet directly beats me
            S2CPartyPartyInviteAcceptNtc inviteAcceptNtc = new S2CPartyPartyInviteAcceptNtc();
            inviteAcceptNtc.ServerId = (ushort) Server.Id;
            inviteAcceptNtc.PartyId = party.Id;
            inviteAcceptNtc.StageId = party.Leader.Client.Character.Stage.Id;
            inviteAcceptNtc.PositionId = 0; // TODO: Figure what this is about
            inviteAcceptNtc.MemberIndex = (byte)partyMember.MemberIndex;
            client.Enqueue(inviteAcceptNtc, queue);

            // Notify party leader of the accepted invitation
            S2CPartyPartyInviteJoinMemberNtc inviteJoinMemberNtc = new S2CPartyPartyInviteJoinMemberNtc();
            CDataPartyMemberMinimum newMemberMinimum = new CDataPartyMemberMinimum();
            GameStructure.CDataCommunityCharacterBaseInfo(newMemberMinimum.CommunityCharacterBaseInfo,
                partyMember.Client.Character);
            newMemberMinimum.IsLeader = partyMember.IsLeader;
            newMemberMinimum.MemberIndex = partyMember.MemberIndex;
            newMemberMinimum.MemberType = partyMember.MemberType;
            newMemberMinimum.PawnId = partyMember.PawnId;
            inviteJoinMemberNtc.MemberMinimumList.Add(newMemberMinimum);
            party.Leader.Client.Enqueue(inviteJoinMemberNtc, queue);

            // Clean up invitation.
            invitation.CancelTimer();
            Server.PartyManager.RemovePartyInvitation(client);

            return queue;
        }
    }
}
