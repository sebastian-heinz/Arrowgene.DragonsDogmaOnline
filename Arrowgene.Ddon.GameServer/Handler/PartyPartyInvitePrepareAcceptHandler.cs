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
            Party newParty = client.PendingInvitedParty; // In case some other thread changes the value
            client.PendingInvitedParty = null;

            client.Send(new S2CPartyPartyInvitePrepareAcceptRes());

            S2CPartyPartyInviteAcceptNtc inviteAcceptNtc = new S2CPartyPartyInviteAcceptNtc();
            inviteAcceptNtc.ServerId = Server.AssetRepository.ServerList[0].Id; // TODO: Get from config, or from DdonGameServer instance
            inviteAcceptNtc.PartyId = newParty.Id;
            inviteAcceptNtc.StageId = newParty.Leader.Stage.Id;
            inviteAcceptNtc.PositionId = 0; // TODO: Figure what this is about
            inviteAcceptNtc.MemberIndex = (byte) newParty.Members.Count;
            client.Send(inviteAcceptNtc);

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