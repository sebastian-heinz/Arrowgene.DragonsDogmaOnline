using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyInviteCharacterHandler : GameStructurePacketHandler<C2SPartyPartyInviteCharacterReq>
    {
        private static readonly ServerLogger Logger =
            LogProvider.Logger<ServerLogger>(typeof(PartyPartyInviteCharacterHandler));

        private static readonly ushort TimeoutSec = 30; // TODO: Move to config?

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
        // TODO PartyGorup need invite management
        public override void Handle(GameClient client, StructurePacket<C2SPartyPartyInviteCharacterReq> packet)
        {
            GameClient invitedClient = Server.ClientLookup.GetClientByCharacterId(packet.Structure.CharacterId);
            if (invitedClient == null)
            {
                Logger.Error(client,
                    $"Could not locate CharacterId:{packet.Structure.CharacterId} for party invitation");
                // TODO error response
                return;
            }

            PartyGroup party = client.Party;
            if (party == null)
            {
                Logger.Error(client,
                    $"can not invite, as he is not inside party");
                // TODO error response
                return;
            }

            PlayerPartyMember invitedMember = party.Invite(invitedClient);
            if (invitedMember == null)
            {
                Logger.Error(client,
                    $"could not invite {invitedClient.Identity}, party did not accept invitiation");
                // TODO error response
                return;
            }

            Logger.Info(client, $"Invited Client:{invitedClient.Identity} to PartyId:{party.Id}");

            S2CPartyPartyInviteNtc ntc = new S2CPartyPartyInviteNtc();
            ntc.TimeoutSec =
                TimeoutSec; // TODO: Implement timeout, send an NTC cancelling the party invite if the timeout is reached
            ntc.PartyListInfo.PartyId = party.Id;
            ntc.PartyListInfo.ServerId = Server.AssetRepository.ServerList[0].Id;
            foreach (PartyMember member in party.Members)
            {
                ntc.PartyListInfo.MemberList.Add(member.GetCDataPartyMember());
            }
            invitedClient.Send(ntc);


            S2CPartyPartyInviteCharacterRes response = new S2CPartyPartyInviteCharacterRes();
            response.TimeoutSec = TimeoutSec; // Same as above
            response.Info.PartyId = party.Id;
            response.Info.ServerId = Server.AssetRepository.ServerList[0].Id;
            response.Info.MemberList.Add(invitedMember.GetCDataPartyMember());
            client.Send(response);
        }
    }
}
