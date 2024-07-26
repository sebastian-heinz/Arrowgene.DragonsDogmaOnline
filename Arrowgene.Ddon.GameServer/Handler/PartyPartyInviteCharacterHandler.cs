using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyInviteCharacterHandler : GameStructurePacketHandler<C2SPartyPartyInviteCharacterReq>
    {
        private static readonly ServerLogger Logger =
            LogProvider.Logger<ServerLogger>(typeof(PartyPartyInviteCharacterHandler));


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
        public override void Handle(GameClient client, StructurePacket<C2SPartyPartyInviteCharacterReq> packet)
        {
            S2CPartyPartyInviteCharacterRes res = new S2CPartyPartyInviteCharacterRes();

            GameClient invitedClient = Server.ClientLookup.GetClientByCharacterId(packet.Structure.CharacterId);
            if (invitedClient == null)
            {
                Logger.Error(client, $"not found CharacterId:{packet.Structure.CharacterId} for party invitation");
                res.Error = (uint)ErrorCode.ERROR_CODE_FAIL;
                client.Send(res);
                return;
            }

            if (invitedClient == client)
            {
                Logger.Error(client, $"can not invite (invitedClient == client)");
                res.Error = (uint)ErrorCode.ERROR_CODE_FAIL;
                client.Send(res);
                return;
            }

            PartyGroup party = client.Party;
            if (party == null)
            {
                Logger.Error(client, "can not invite (client.Party == null)");
                res.Error = (uint)ErrorCode.ERROR_CODE_FAIL;
                client.Send(res);
                return;
            }

            ErrorRes<PlayerPartyMember> invitedMember = party.Invite(invitedClient, client);
            if (invitedMember.HasError)
            {
                Logger.Error(client, $"could not invite {invitedClient.Identity}");
                res.Error = (uint)invitedMember.ErrorCode;
                client.Send(res);
                return;
            }

            S2CPartyPartyInviteNtc ntc = new S2CPartyPartyInviteNtc();
            ntc.TimeoutSec = PartyManager.InvitationTimeoutSec;
            ntc.PartyListInfo.PartyId = party.Id;
            ntc.PartyListInfo.ServerId = Server.AssetRepository.ServerList[0].Id;
            foreach (PartyMember member in party.Members)
            {
                ntc.PartyListInfo.MemberList.Add(member.GetCDataPartyMember());
            }

            invitedClient.Send(ntc);


            res.TimeoutSec = PartyManager.InvitationTimeoutSec;
            res.Info.PartyId = party.Id;
            res.Info.ServerId = Server.AssetRepository.ServerList[0].Id;
            res.Info.MemberList.Add(invitedMember.Value.GetCDataPartyMember());
            client.Send(res);

            Logger.Info(client, $"Invited Client:{invitedClient.Identity} to PartyId:{party.Id}");
        }
    }
}
