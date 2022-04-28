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
            client.Send(new S2CPartyPartyInvitePrepareAcceptRes());

            client.PendingInvitedParty.Members.Add(client);
            client.Party = client.PendingInvitedParty;
            client.PendingInvitedParty = null;

            S2CPartyPartyInviteAcceptNtc inviteAcceptNtc = new S2CPartyPartyInviteAcceptNtc();
            inviteAcceptNtc.ServerId = 0; // TODO: Get from config, or from DdonGameServer instance
            inviteAcceptNtc.PartyId = client.Party.Id;
            inviteAcceptNtc.StageId = client.Party.Leader.Stage.Id;
            inviteAcceptNtc.PositionId = 0; // TODO: Figure what this is about
            inviteAcceptNtc.MemberIndex = (byte) client.Party.Members.IndexOf(client);
            foreach(GameClient member in client.Party.Members)
            {
                member.Send(inviteAcceptNtc);
            }
            
            CDataPartyMemberMinimum partyMemberMinimum = new CDataPartyMemberMinimum();
            partyMemberMinimum.CommunityCharacterBaseInfo.CharacterId = client.Character.Id;
            partyMemberMinimum.CommunityCharacterBaseInfo.CharacterName.FirstName = client.Character.FirstName;
            partyMemberMinimum.CommunityCharacterBaseInfo.CharacterName.LastName = client.Character.LastName;
            partyMemberMinimum.CommunityCharacterBaseInfo.ClanName = "XYZ"; // TODO: Clan name in the Character object
            partyMemberMinimum.MemberIndex = client.Party.Members.IndexOf(client);
            partyMemberMinimum.MemberType = 0; // TODO: Figure out values
            partyMemberMinimum.PawnId = 0;
            partyMemberMinimum.IsLeader = client == client.Party.Leader;
            S2CPartyPartyInviteJoinMemberNtc partyMemberJoinNtc = new S2CPartyPartyInviteJoinMemberNtc();
            partyMemberJoinNtc.MemberMinimumList.Add(partyMemberMinimum);
            foreach (GameClient member in client.Party.Members)
            {
                if(member != client)
                {
                    member.Send(partyMemberJoinNtc);
                }
            }

            S2CPartyPartyJoinNtc partyJoinNtc = new S2CPartyPartyJoinNtc();
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
                partyJoinNtc.PartyMembers.Add(partyMember);
            }
            client.Send(partyJoinNtc);
        }
    }
}