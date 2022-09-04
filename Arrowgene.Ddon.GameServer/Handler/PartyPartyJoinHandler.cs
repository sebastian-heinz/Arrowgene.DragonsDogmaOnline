using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
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
                partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.FirstName = member.Character.CharacterInfo.FirstName;
                partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.LastName = member.Character.CharacterInfo.LastName;
                partyMember.CharacterListElement.CurrentJobBaseInfo.Job = member.Character.CharacterInfo.Job;
                partyMember.CharacterListElement.CurrentJobBaseInfo.Level = (byte) member.Character.CharacterInfo.CharacterJobDataList
                    .Where(x => x.Job == member.Character.CharacterInfo.Job)
                    .Select(x => x.Lv)
                    .SingleOrDefault();
                partyMember.CharacterListElement.OnlineStatus = member.OnlineStatus;
                partyMember.CharacterListElement.unk2 = 1;
                partyMember.MemberType = 1;
                partyMember.MemberIndex = (byte) i;
                partyMember.IsLeader = member.Character.Id == newParty.Leader.Character.Id;
                partyMember.JoinState = JoinState.On;
                partyJoinNtcForNewMember.PartyMembers.Add(partyMember);
            }
            newParty.SendToAll(partyJoinNtcForNewMember);

            // Send party player context NTCs to the new member
            for(byte i = 0; i < newParty.Members.Count; i++)
            {
                GameClient member = newParty.Members[i];
                S2CContextGetPartyPlayerContextNtc partyPlayerContextNtc = new S2CContextGetPartyPlayerContextNtc(member.Character);
                partyPlayerContextNtc.Context.Base.MemberIndex = i;
                newParty.SendToAll(partyPlayerContextNtc);
            }
        }
    }
}