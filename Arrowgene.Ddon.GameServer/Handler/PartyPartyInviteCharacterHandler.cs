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
        // 1.   S->C    S2CPartyPartyInviteNtc to the player
        // 2.   C->S    The player accepts the invite, sending a C2SPartyPartyInvitePrepareAcceptReq
        // 3.   S->C    S2CPartyPartyInviteAcceptNtc to the player. 
        // 4.   C->S    The player teleports to the party leader and leaves the previous party, sending a C2SPartyPartyLeaveReq
        // 5.   S->C    S2CPartyPartyLeaveNtc to the old party members
        // 6.   C->S    The player requests to join the new party, sending a C2SPartyPartyJoinReq
        // 7. To determine
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
            ntc.PartyListInfo.PartyId = client.Party.Id;
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
                partyMember.IsLeader = member == client.Party.Leader;
                ntc.PartyListInfo.MemberList.Add(partyMember);
            }
            targetClient.Send(ntc);

            S2CPartyPartyInviteCharacterRes response = new S2CPartyPartyInviteCharacterRes();
            response.TimeoutSec = ntc.TimeoutSec;
            response.Info = ntc.PartyListInfo;
            client.Send(response);
        }
    }
}