using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyInviteCharacterHandler : GameRequestPacketHandler<C2SPartyPartyInviteCharacterReq, S2CPartyPartyInviteCharacterRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyInviteCharacterHandler));

        public PartyPartyInviteCharacterHandler(DdonGameServer server) : base(server)
        {
        }

        // When a player is invited:
        //  PartyPartyInviteCharacterHandler
        // 0.   C->S    The party leader sends an invite to another player, sending a C2SPartyPartyInviteCharacterReq
        // 1.   S->C    S2CPartyPartyInviteNtc to the player, C2SPartyPartyInviteCharacterRes to the inviter with the new players info
        //  PartyPartyInvitePrepareAcceptHandler
        // 2.   C->S    The player accepts the invite, sending a C2SPartyPartyInvitePrepareAcceptReq
        // 3.   S->C    S2CPartyPartyInviteAcceptNtc to the player so they move to the leader's server.
        //              S2CPartyPartyInviteJoinMemberNtc to the leader so they know the invite was accepted.
        //  PartyPartyLeaveHandler
        // 4.   C->S    The player teleports to the party leader and leaves the previous party, sending a C2SPartyPartyLeaveReq
        // 5.   S->C    S2CPartyPartyLeaveNtc to the old party members
        //  PartyPartyJoinHandler
        // 6.   C->S    The player requests to join the new party, sending a C2SPartyPartyJoinReq
        // 7.   S->C    S2CPartyPartyJoinNtc to all members
        //              S2CContextGetLobbyPlayerContextNtc to the new party member, it should maybe also be sent to all members
        //              More stuff to determine
        // TODO: Figure out just how much packets/data within those packets we can do without while keeping everything functioning.
        public override S2CPartyPartyInviteCharacterRes Handle(GameClient client, C2SPartyPartyInviteCharacterReq request)
        {

            GameClient invitedClient = Server.ClientLookup.GetClientByCharacterId(request.CharacterId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_INVITE_FAIL_REASON_MEMBER_NOT_FOUND,
                $"not found CharacterId:{request.CharacterId} for party invitation");
            
            if (invitedClient == client)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_INTERNAL_ERROR, $"can not invite (invitedClient == client)");
            }

            if (client.GameMode == GameMode.Normal && !invitedClient.Character.HasContentReleased(ContentsRelease.PartyPlayers))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_CONTENTS_RELEASE_NOT_PARTY_PLAY_WITH_PLAYER, "unable to invite to party (party play not unlocked)");
            }

            PartyGroup party = client.Party
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_PARTY_NOT_FOUNDED, "can not invite (client.Party == null)");

            PlayerPartyMember invitedMember = party.Invite(invitedClient, client);

            S2CPartyPartyInviteNtc ntc = new()
            {
                TimeoutSec = PartyManager.InvitationTimeoutSec,
                PartyListInfo = new()
                {
                    PartyId = party.Id,
                    ServerId = (uint)Server.Id,
                    MemberList = [.. party.Members.Select(x => x.GetCDataPartyMember())]
                }
            };

            invitedClient.Send(ntc);

            S2CPartyPartyInviteCharacterRes res = new S2CPartyPartyInviteCharacterRes
            {
                TimeoutSec = PartyManager.InvitationTimeoutSec,
                Info = new()
                {
                    PartyId = party.Id,
                    ServerId = (uint)Server.Id,
                    MemberList = [invitedMember.GetCDataPartyMember()]
                }
            };

            Logger.Info(client, $"Invited Client:{invitedClient.Identity} to PartyId:{party.Id}");

            return res;
        }
    }
}
