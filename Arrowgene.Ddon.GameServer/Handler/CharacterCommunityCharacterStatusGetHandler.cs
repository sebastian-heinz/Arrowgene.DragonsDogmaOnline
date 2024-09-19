using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterCommunityCharacterStatusGetHandler : GameStructurePacketHandler<C2SCharacterCommunityCharacterStatusGetReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterCommunityCharacterStatusGetHandler));


        public CharacterCommunityCharacterStatusGetHandler(DdonGameServer server) : base(server)
        {
        }

        // public override PacketId Id => PacketId.C2S_CHARACTER_COMMUNITY_CHARACTER_STATUS_GET_REQ;

        public override void Handle(GameClient client, StructurePacket<C2SCharacterCommunityCharacterStatusGetReq> packet)
        {
            // client.Send(InGameDump.Dump_65);

            Logger.Info($"Update request type {packet.Structure.unType}");

            List<CDataCharacterListElement> updateCharacterList = new List<CDataCharacterListElement>();
            List<CDataUpdateMatchingProfileInfo> updateMatchingProfileList = new List<CDataUpdateMatchingProfileInfo>();
            
            List<ContactListEntity> friends = Database.SelectContactsByCharacterId(client.Character.CharacterId);
            
            foreach (var f in friends)
            {
                if (f.Type != ContactListType.FriendList || f.Status != ContactListStatus.Accepted)
                {
                    continue;
                }
                Character otherCharacter =
                    ContactListManager.getCharWithOnlineStatus(Server,Database, f.GetOtherCharacterId(client.Character.CharacterId));
                updateCharacterList.Add(ContactListManager.CharacterToListEml(otherCharacter));
                updateMatchingProfileList.Add(new CDataUpdateMatchingProfileInfo()
                {
                    CharacterId = otherCharacter.CharacterId,
                    Comment = otherCharacter.MatchingProfile.Comment
                });
            }
            
            client.Send(new S2CCharacterCommunityCharacterStatusUpdateNtc()
            {
                UpdateCharacterList = updateCharacterList,
                UpdateMatchingProfileList = updateMatchingProfileList
            });
            
            client.Send(new S2CCharacterCommunityCharacterStatusGetRes()
            {
                Result = updateCharacterList.Count + 1
            });
        }
    }
}
