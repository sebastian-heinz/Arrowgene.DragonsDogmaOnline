using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class CharacterCommunityCharacterStatusGetHandler : GameRequestPacketHandler<C2SCharacterCommunityCharacterStatusGetReq, S2CCharacterCommunityCharacterStatusGetRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(CharacterCommunityCharacterStatusGetHandler));

        public CharacterCommunityCharacterStatusGetHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CCharacterCommunityCharacterStatusGetRes Handle(GameClient client, C2SCharacterCommunityCharacterStatusGetReq request)
        {
            List<CDataCharacterListElement> updateCharacterList = new List<CDataCharacterListElement>();
            List<CDataUpdateMatchingProfileInfo> updateMatchingProfileList = new List<CDataUpdateMatchingProfileInfo>();

            List<(ContactListEntity, CDataCharacterListElement)> friends = Database.SelectFullContactListByCharacterId(client.Character.CharacterId);

            foreach ((ContactListEntity contact, CDataCharacterListElement character) in friends)
            {
                if (contact.Type != ContactListType.FriendList || contact.Status != ContactListStatus.Accepted)
                {
                    continue;
                }

                var matchClient = Server.ClientLookup.GetClientByCharacterId(character.CommunityCharacterBaseInfo.CharacterId);
                if (matchClient != null)
                {
                    character.OnlineStatus = matchClient.Character?.OnlineStatus ?? OnlineStatus.Offline;
                    character.ServerId = (ushort)Server.Id;
                }
                else
                {
                    var matchServer = Server.RpcManager.FindPlayerById(character.CommunityCharacterBaseInfo.CharacterId);
                    if (matchServer != 0)
                    {
                        character.OnlineStatus = OnlineStatus.Online; // TODO
                        character.ServerId = matchServer;
                    }
                    else
                    {
                        character.OnlineStatus = OnlineStatus.Offline;
                    }
                }

                updateCharacterList.Add(character);
                updateMatchingProfileList.Add(new CDataUpdateMatchingProfileInfo()
                {
                    CharacterId = character.CommunityCharacterBaseInfo.CharacterId,
                    Comment = character.MatchingProfile,
                });
            }

            client.Send(new S2CCharacterCommunityCharacterStatusUpdateNtc()
            {
                UpdateCharacterList = updateCharacterList,
                UpdateMatchingProfileList = updateMatchingProfileList
            });

            return new S2CCharacterCommunityCharacterStatusGetRes()
            {
                Result = (uint)(updateCharacterList.Count + 1) // ???
            };
        }
    }
}
