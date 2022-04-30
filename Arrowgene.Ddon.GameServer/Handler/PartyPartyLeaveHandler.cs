using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class PartyPartyLeaveHandler : StructurePacketHandler<GameClient, C2SPartyPartyLeaveReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyPartyLeaveHandler));

        public PartyPartyLeaveHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SPartyPartyLeaveReq> packet)
        {
            Party oldParty = client.Party;

            client.Send(new S2CPartyPartyLeaveRes());

            S2CPartyPartyLeaveNtc partyLeaveNtc = new S2CPartyPartyLeaveNtc();
            partyLeaveNtc.CharacterId = client.Character.Id;
            foreach (GameClient member in oldParty.Members)
            {
                member.Send(partyLeaveNtc);
            }

            oldParty.Members.Remove(client);
            client.Party = null;

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
        }
    }
}