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

            newParty.Members.Add(client);
            client.PendingInvitedParty = null;
            client.Party = newParty;

            S2CPartyPartyJoinRes response = new S2CPartyPartyJoinRes();
            response.PartyId = newParty.Id;
            client.Send(response);

            // Send members to new member
            S2CPartyPartyJoinNtc partyJoinNtcForNewMember = new S2CPartyPartyJoinNtc();
            partyJoinNtcForNewMember.HostCharacterId = newParty.Host.Character.Id;
            partyJoinNtcForNewMember.LeaderCharacterId = newParty.Leader.Character.Id;
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
                partyMember.CharacterListElement.OnlineStatus = member.OnlineStatus;
                partyMember.CharacterListElement.unk2 = 1;
                partyMember.MemberType = 1;
                partyMember.MemberIndex = (byte) i;
                partyMember.IsLeader = member.Character.Id == newParty.Leader.Character.Id;
                partyMember.JoinState = JoinState.On;
                partyJoinNtcForNewMember.PartyMembers.Add(partyMember);
            }
            newParty.SendToAll(partyJoinNtcForNewMember);

            // Send party player context NTCs
            S2CContextGetLobbyPlayerContextNtc sampleData = EntitySerializer
                        .Get<S2CContextGetLobbyPlayerContextNtc>().Read(SelectedDump.data_Dump_LobbyPlayerContext);
            foreach(GameClient member in newParty.Members)
            {
                S2CContextGetPartyPlayerContextNtc partyPlayerContextNtc = new S2CContextGetPartyPlayerContextNtc();
                partyPlayerContextNtc.CharacterId = member.Character.Id;
                partyPlayerContextNtc.Context.Base = sampleData.Context.Base;
                partyPlayerContextNtc.Context.Base.CharacterId = member.Character.Id;
                partyPlayerContextNtc.Context.Base.FirstName = member.Character.FirstName;
                partyPlayerContextNtc.Context.Base.LastName = member.Character.LastName;
                partyPlayerContextNtc.Context.PlayerInfo = sampleData.Context.PlayerInfo;
                partyPlayerContextNtc.Context.ResistInfo = new CDataContextResist();
                partyPlayerContextNtc.Context.EditInfo = member.Character.Visual;
                client.Send(partyPlayerContextNtc);
            }
        }
    }
}