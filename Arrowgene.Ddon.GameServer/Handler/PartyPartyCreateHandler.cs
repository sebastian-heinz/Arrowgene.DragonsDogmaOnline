using System.Reflection.Metadata;
using Arrowgene.Ddon.GameServer.Dump;
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
            client.Party = new Party();
            client.Party.Members.Add(client);
            client.Party.Host = client;
            client.Party.Leader = client;
            
            S2CPartyPartyJoinNtc partyJoinNtc = new S2CPartyPartyJoinNtc();
            partyJoinNtc.HostCharacterId = client.Character.Id;
            partyJoinNtc.LeaderCharacterId = client.Character.Id;
            CDataPartyMember partyMember = new CDataPartyMember();
            partyMember.CharacterListElement.ServerId = Server.AssetRepository.ServerList[0].Id;
            partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterId = client.Character.Id;
            partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.FirstName = client.Character.FirstName;
            partyMember.CharacterListElement.CommunityCharacterBaseInfo.CharacterName.LastName = client.Character.LastName;
            partyMember.CharacterListElement.CurrentJobBaseInfo.Job = Server.AssetRepository.ArisenAsset[0].Job;
            partyMember.CharacterListElement.CurrentJobBaseInfo.Level = (byte) Server.AssetRepository.ArisenAsset[0].Lv;
            partyMember.CharacterListElement.EntryJobBaseInfo.Job = Server.AssetRepository.ArisenAsset[0].Job;
            partyMember.CharacterListElement.EntryJobBaseInfo.Level = (byte) Server.AssetRepository.ArisenAsset[0].Lv;
            partyJoinNtc.PartyMembers.Add(partyMember);
            client.Send(partyJoinNtc);
           
            S2CPartyPartyCreateRes partyCreateRes = new S2CPartyPartyCreateRes();
            partyCreateRes.PartyId = client.Party.Id;
            client.Send(partyCreateRes);

            //client.Send(InGameDump.Dump_103);
            
            //client.Send(InGameDump.Dump_104);
            //client.Send(InGameDump.Dump_105);
        }
    }
}
