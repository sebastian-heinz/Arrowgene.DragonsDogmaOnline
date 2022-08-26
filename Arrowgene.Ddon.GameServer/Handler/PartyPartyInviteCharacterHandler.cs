using System.Linq;
using System.Reflection.Metadata;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyInviteCharacterHandler : StructurePacketHandler<GameClient, C2SPartyPartyInviteCharacterReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyInviteCharacterHandler));

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
        public override void Handle(GameClient client, StructurePacket<C2SPartyPartyInviteCharacterReq> packet)
        {
            // TODO: Optimize this lmao
            GameClient targetClient = Server.Clients.Where(x => x.Character.Id == packet.Structure.CharacterId).First();
            // TODO: What would happen if two parties are trying to invite the same character?
            targetClient.PendingInvitedParty = client.Party;

            S2CPartyPartyInviteNtc ntc = new S2CPartyPartyInviteNtc();
            ntc.TimeoutSec = TimeoutSec; // TODO: Implement timeout, send an NTC cancelling the party invite if the timeout is reached
            ntc.PartyListInfo.PartyId = client.Party.Id;
            ntc.PartyListInfo.ServerId = Server.AssetRepository.ServerList[0].Id;
            for(int i = 0; i < client.Party.Members.Count; i++)
            {
                GameClient member = client.Party.Members[i];
                CDataPartyMember partyMember = new CDataPartyMember();
                partyMember.CharacterListElement.ServerId = Server.AssetRepository.ServerList[0].Id;
                partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterId = member.Character.Id;
                partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.FirstName = member.Character.FirstName;
                partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.LastName = member.Character.LastName;
                partyMember.CharacterListElement.CurrentJobBaseInfo.Job = Server.AssetRepository.ArisenAsset[0].Job;
                partyMember.CharacterListElement.CurrentJobBaseInfo.Level = (byte) Server.AssetRepository.ArisenAsset[0].Lv;
                partyMember.CharacterListElement.EntryJobBaseInfo.Job = Server.AssetRepository.ArisenAsset[0].Job;
                partyMember.CharacterListElement.EntryJobBaseInfo.Level = (byte) Server.AssetRepository.ArisenAsset[0].Lv;
                partyMember.IsLeader = member.Character.Id == client.Party.Leader.Character.Id;
                partyMember.MemberIndex = (byte) i;
                ntc.PartyListInfo.MemberList.Add(partyMember);
            }
            targetClient.Send(ntc);

            S2CPartyPartyInviteCharacterRes response = new S2CPartyPartyInviteCharacterRes();
            response.TimeoutSec = TimeoutSec; // Same as above
            response.Info.PartyId = targetClient.Party.Id;
            response.Info.ServerId = Server.AssetRepository.ServerList[0].Id;
            CDataPartyMember otherPartyMember = new CDataPartyMember();
            otherPartyMember.CharacterListElement.ServerId = Server.AssetRepository.ServerList[0].Id;
            otherPartyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterId = targetClient.Character.Id;
            otherPartyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.FirstName = targetClient.Character.FirstName;
            otherPartyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.LastName = targetClient.Character.LastName;
            otherPartyMember.CharacterListElement.CurrentJobBaseInfo.Job = Server.AssetRepository.ArisenAsset[0].Job;
            otherPartyMember.CharacterListElement.CurrentJobBaseInfo.Level = (byte) Server.AssetRepository.ArisenAsset[0].Lv;
            otherPartyMember.CharacterListElement.EntryJobBaseInfo.Job = Server.AssetRepository.ArisenAsset[0].Job;
            otherPartyMember.CharacterListElement.EntryJobBaseInfo.Level = (byte) Server.AssetRepository.ArisenAsset[0].Lv;
            otherPartyMember.IsLeader = targetClient.Character.Id == client.Party.Leader.Character.Id;
            otherPartyMember.MemberIndex = 0;
            response.Info.MemberList.Add(otherPartyMember);
            client.Send(response);
        }
    }
}