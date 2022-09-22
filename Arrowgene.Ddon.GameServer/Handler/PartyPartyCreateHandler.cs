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
    public class PartyPartyCreateHandler : StructurePacketHandler<GameClient, C2SPartyPartyCreateReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyCreateHandler));

        public PartyPartyCreateHandler(DdonGameServer server) : base(server)
        {
                              
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartyPartyCreateReq> packet)
        {
            Party newParty = ((DdonGameServer) Server).NewParty();
            newParty.Members.Add(client);
            newParty.Host = client;
            newParty.Leader = client;
            client.Party = newParty;
            
            S2CPartyPartyJoinNtc partyJoinNtc = new S2CPartyPartyJoinNtc();
            partyJoinNtc.HostCharacterId = client.Character.Id;
            partyJoinNtc.LeaderCharacterId = client.Character.Id;
            CDataPartyMember partyMember = new CDataPartyMember();
            partyMember.CharacterListElement.ServerId = Server.AssetRepository.ServerList[0].Id;
            partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterId = client.Character.Id;
            partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.FirstName = client.Character.FirstName;
            partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.LastName = client.Character.LastName;
            partyMember.CharacterListElement.CurrentJobBaseInfo.Job = client.Character.Job;
            partyMember.CharacterListElement.CurrentJobBaseInfo.Level = (byte) client.Character.ActiveCharacterJobData.Lv;
            partyMember.CharacterListElement.OnlineStatus = client.Character.OnlineStatus;
            partyMember.CharacterListElement.unk2 = 1;
            partyMember.MemberType = 1;
            partyMember.IsLeader = newParty.Leader == client;
            partyMember.JoinState = JoinState.On;
            partyJoinNtc.PartyMembers.Add(partyMember);
            client.Send(partyJoinNtc);
           
            S2CPartyPartyCreateRes partyCreateRes = new S2CPartyPartyCreateRes();
            partyCreateRes.PartyId = newParty.Id;
            client.Send(partyCreateRes);
        }
    }
}
