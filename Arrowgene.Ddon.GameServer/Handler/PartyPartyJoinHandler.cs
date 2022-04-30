using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyJoinHandler : StructurePacketHandler<GameClient, C2SPartyPartyJoinReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyJoinHandler));

        public PartyPartyJoinHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartyPartyJoinReq> packet)
        {
            // TODO: Optimize
            Party newParty = ((DdonGameServer) Server).Parties.Find(x => x.Id == packet.Structure.PartyId);

            S2CPartyPartyJoinRes response = new S2CPartyPartyJoinRes();
            response.PartyId = newParty.Id;
            client.Send(response);


            // Inform already present members of the new member
            S2CContextGetLobbyPlayerContextNtc sampleData = EntitySerializer
                        .Get<S2CContextGetLobbyPlayerContextNtc>().Read(SelectedDump.data_Dump_LobbyPlayerContext);
            S2CContextGetPartyPlayerContextNtc partyPlayerContextNtc = new S2CContextGetPartyPlayerContextNtc();
            partyPlayerContextNtc.CharacterId = client.Character.Id;
            partyPlayerContextNtc.Context.Base = sampleData.Context.Base;
            partyPlayerContextNtc.Context.Base.CharacterId = client.Character.Id;
            partyPlayerContextNtc.Context.Base.FirstName = client.Character.FirstName;
            partyPlayerContextNtc.Context.Base.LastName = client.Character.LastName;
            partyPlayerContextNtc.Context.PlayerInfo = sampleData.Context.PlayerInfo;
            partyPlayerContextNtc.Context.ResistInfo = new CDataContextResist();
            partyPlayerContextNtc.Context.EditInfo = client.Character.Visual;

            foreach (GameClient member in newParty.Members)
            {
                member.Send(partyPlayerContextNtc);
            }


            newParty.Members.Add(client);
            client.PendingInvitedParty = null;
            client.Party = newParty;


            // Inform new member of the already present members
            S2CPartyPartyJoinNtc partyJoinNtc = new S2CPartyPartyJoinNtc();
            partyJoinNtc.HostCharacterId = newParty.Host.Character.Id;
            partyJoinNtc.LeaderCharacterId = newParty.Leader.Character.Id;
            for(int i = 0; i < newParty.Members.Count; i++)
            {
                GameClient member = newParty.Members[i];
                CDataPartyMember partyMember = new CDataPartyMember();
                partyMember.CharacterListElement.ServerId = Server.AssetRepository.ServerList[0].Id;
                partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterId = member.Character.Id;
                partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.FirstName = member.Character.FirstName;
                partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.LastName = member.Character.LastName;
                partyMember.CharacterListElement.CurrentJobBaseInfo.Job = Server.AssetRepository.ArisenAsset[0].Job;
                partyMember.CharacterListElement.CurrentJobBaseInfo.Level = (byte) Server.AssetRepository.ArisenAsset[0].Lv;
                partyMember.CharacterListElement.EntryJobBaseInfo.Job = Server.AssetRepository.ArisenAsset[0].Job;
                partyMember.CharacterListElement.EntryJobBaseInfo.Level = (byte) Server.AssetRepository.ArisenAsset[0].Lv;
                partyMember.IsLeader = member.Character.Id == newParty.Leader.Character.Id;
                partyMember.MemberIndex = (byte) i;
                partyJoinNtc.PartyMembers.Add(partyMember);
            }
            client.Send(partyJoinNtc);

            // Somehow related to parties
            // In the pcaps its seen twice (out of three times) right after a party join NTC
            client.Send(new S2CContext_35_15_16_Ntc());

            CDataCharacterListElement characterListElement = new CDataCharacterListElement();
            characterListElement.ServerId = Server.AssetRepository.ServerList[0].Id;
            characterListElement.CommunityCharacterBaseInfo.CharacterId = client.Character.Id;
            characterListElement.CommunityCharacterBaseInfo.CharacterName.FirstName = client.Character.FirstName;
            characterListElement.CommunityCharacterBaseInfo.CharacterName.LastName = client.Character.LastName;
            characterListElement.CommunityCharacterBaseInfo.ClanName = "123";
            characterListElement.CurrentJobBaseInfo.Job = Server.AssetRepository.ArisenAsset[0].Job;
            characterListElement.CurrentJobBaseInfo.Level = (byte) Server.AssetRepository.ArisenAsset[0].Lv;
            characterListElement.EntryJobBaseInfo.Job = Server.AssetRepository.ArisenAsset[0].Job;
            characterListElement.EntryJobBaseInfo.Level = (byte) Server.AssetRepository.ArisenAsset[0].Lv;
            CDataUpdateMatchingProfileInfo updateMatchingProfileInfo = new CDataUpdateMatchingProfileInfo();
            updateMatchingProfileInfo.CharacterId = client.Character.Id;
            S2CCharacterCommunityCharacterStatusUpdateNtc statusUpdateNtc = new S2CCharacterCommunityCharacterStatusUpdateNtc();
            statusUpdateNtc.UpdateCharacterList.Add(characterListElement);
            statusUpdateNtc.UpdateMatchingProfileList.Add(updateMatchingProfileInfo);
            client.Send(statusUpdateNtc); 

            foreach(GameClient member in newParty.Members)
            {
                // Not sending the new member's info to themself makes them not be able to jump/open menus
                S2CContextGetPartyPlayerContextNtc alreadyPresentPartyPlayerContextNtc = new S2CContextGetPartyPlayerContextNtc();
                alreadyPresentPartyPlayerContextNtc.CharacterId = member.Character.Id;
                alreadyPresentPartyPlayerContextNtc.Context.Base = sampleData.Context.Base;
                alreadyPresentPartyPlayerContextNtc.Context.Base.CharacterId = member.Character.Id;
                alreadyPresentPartyPlayerContextNtc.Context.Base.FirstName = member.Character.FirstName;
                alreadyPresentPartyPlayerContextNtc.Context.Base.LastName = member.Character.LastName;
                alreadyPresentPartyPlayerContextNtc.Context.PlayerInfo = sampleData.Context.PlayerInfo;
                alreadyPresentPartyPlayerContextNtc.Context.ResistInfo = new CDataContextResist();
                alreadyPresentPartyPlayerContextNtc.Context.EditInfo = member.Character.Visual;
                client.Send(alreadyPresentPartyPlayerContextNtc);

                if(member != client) {
                    // Sending the new member's info to themself makes them not be able to jump/open menus
                    S2CQuestPartyQuestProgressNtc partyQuestProgressNtc = new S2CQuestPartyQuestProgressNtc();
                    partyQuestProgressNtc.ProgressCharacterId = member.Character.Id;
                    client.Send(partyQuestProgressNtc);
                }
            }
        }
    }
}