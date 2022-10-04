using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
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

            C2SPartyPartyInvitePrepareAcceptReq req = packet.Structure;

            IBuffer buf = packet.AsBuffer();
            Logger.Hex(packet.Data);
            
            // client == invited client
            PartyGroup party = Server.PartyManager.GetInvitedParty(client);
            if (party == null)
            {
                Logger.Error(client, "failed to find invited party");
                // TODO error resp
                return;
            }

            PlayerPartyMember partyMember = party.Accept(client);
            if (partyMember == null)
            {
                Logger.Error(client, "failed to accept invite");
                // TODO error resp
                return;
            }
            
            Logger.Info(client, $"Accepted Invite for PartyId:{party.Id}");

            
            client.Send(new S2CPartyPartyInvitePrepareAcceptRes());


            // The invited player doesn't move to the new party leader's server until this packet is sent
            // Why this wasn't included in the Response packet directly beats me
            S2CPartyPartyInviteAcceptNtc inviteAcceptNtc = new S2CPartyPartyInviteAcceptNtc();
            inviteAcceptNtc.ServerId =
                Server.AssetRepository.ServerList[0].Id; // TODO: Get from config, or from DdonGameServer instance
            inviteAcceptNtc.PartyId = party.Id;
            inviteAcceptNtc.StageId = party.Leader.Character.Stage.Id;
            inviteAcceptNtc.PositionId = 0; // TODO: Figure what this is about
            inviteAcceptNtc.MemberIndex = (byte)partyMember.MemberIndex;
            client.Send(inviteAcceptNtc);

            // Notify party leader of the accepted invitation
            S2CPartyPartyInviteJoinMemberNtc inviteJoinMemberNtc = new S2CPartyPartyInviteJoinMemberNtc();
            CDataPartyMemberMinimum newMemberMinimum = new CDataPartyMemberMinimum();
            GameStructure.CDataCommunityCharacterBaseInfo(newMemberMinimum.CommunityCharacterBaseInfo,
                partyMember.Character);
            newMemberMinimum.IsLeader = partyMember.IsLeader;
            newMemberMinimum.MemberIndex = partyMember.MemberIndex;
            newMemberMinimum.MemberType = partyMember.MemberType;
            newMemberMinimum.PawnId = partyMember.PawnId;
            inviteJoinMemberNtc.MemberMinimumList.Add(newMemberMinimum);
            party.Leader.Client.Send(inviteJoinMemberNtc);
        }
    }
}
