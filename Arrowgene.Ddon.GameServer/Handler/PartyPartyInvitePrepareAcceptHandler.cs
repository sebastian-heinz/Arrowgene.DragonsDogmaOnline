using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyInvitePrepareAcceptHandler : GameStructurePacketHandler<C2SPartyPartyInvitePrepareAcceptReq>
    {
        private static readonly ServerLogger Logger =
            LogProvider.Logger<ServerLogger>(typeof(PartyPartyInvitePrepareAcceptHandler));

        public PartyPartyInvitePrepareAcceptHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartyPartyInvitePrepareAcceptReq> packet)
        {
            S2CPartyPartyInvitePrepareAcceptRes res = new S2CPartyPartyInvitePrepareAcceptRes();

            PartyInvitation invitation = Server.PartyManager.GetPartyInvitation(client);
            if (invitation == null)
            {
                Logger.Error(client, "failed to find invitation");
                client.Send(res);
                return;
            }

            PartyGroup party = invitation.Party;
            if (party == null)
            {
                Logger.Error(client, "failed to find invited party");
                client.Send(res);
                return;
            }

            ErrorRes<PlayerPartyMember> partyMember = party.Accept(client);
            if (partyMember.HasError)
            {
                Logger.Error(client, "failed to accept invite");
                res.Error = (uint)partyMember.ErrorCode;
                client.Send(res);
                return;
            }

            Logger.Info(client, $"Accepted Invite for PartyId:{party.Id}");
            client.Send(res);


            // The invited player doesn't move to the new party leader's server until this packet is sent
            // Why this wasn't included in the Response packet directly beats me
            S2CPartyPartyInviteAcceptNtc inviteAcceptNtc = new S2CPartyPartyInviteAcceptNtc();
            inviteAcceptNtc.ServerId =
                Server.AssetRepository.ServerList[0].Id; // TODO: Get from config, or from DdonGameServer instance
            inviteAcceptNtc.PartyId = party.Id;
            inviteAcceptNtc.StageId = party.Leader.Character.Stage.Id;
            inviteAcceptNtc.PositionId = 0; // TODO: Figure what this is about
            inviteAcceptNtc.MemberIndex = (byte)partyMember.Value.MemberIndex;
            client.Send(inviteAcceptNtc);

            // Notify party leader of the accepted invitation
            S2CPartyPartyInviteJoinMemberNtc inviteJoinMemberNtc = new S2CPartyPartyInviteJoinMemberNtc();
            CDataPartyMemberMinimum newMemberMinimum = new CDataPartyMemberMinimum();
            GameStructure.CDataCommunityCharacterBaseInfo(newMemberMinimum.CommunityCharacterBaseInfo,
                partyMember.Value.Character);
            newMemberMinimum.IsLeader = partyMember.Value.IsLeader;
            newMemberMinimum.MemberIndex = partyMember.Value.MemberIndex;
            newMemberMinimum.MemberType = partyMember.Value.MemberType;
            newMemberMinimum.PawnId = partyMember.Value.PawnId;
            inviteJoinMemberNtc.MemberMinimumList.Add(newMemberMinimum);
            party.Leader.Client.Send(inviteJoinMemberNtc);
        }
    }
}
