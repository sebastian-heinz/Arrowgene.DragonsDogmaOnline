using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class FriendGetFriendListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(FriendGetFriendListHandler));


        public FriendGetFriendListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_FRIEND_GET_FRIEND_LIST_REQ;
            // S2C_FRIEND_GET_FRIEND_LIST_RES
            // S2CFriendGetFriendListRes

        public override void Handle(GameClient client, IPacket packet)
        {
            // client.Send(InGameDump.Dump_60);
            List<CDataFriendInfo> fList = new List<CDataFriendInfo>();
            List<CDataCommunityCharacterBaseInfo> applList = new List<CDataCommunityCharacterBaseInfo>();
            List<CDataCommunityCharacterBaseInfo> apprList = new List<CDataCommunityCharacterBaseInfo>();
            List<ContactListEntity> friends = Database.SelectFriends((int) client.Character.CharacterId);
            
            foreach (var f in friends)
            {
                if (f.Type != ContactListType.FriendList) continue;
                Character otherCharacter =
                    Database.SelectCharacter((uint)f.GetOtherCharacterId((int)client.Character.CharacterId));
                
                if (f.Status == ContactListStatus.Accepted)
                {
                    fList.Add(CharacterToFriendInfo(otherCharacter));
                } 
                else if (f.Status == ContactListStatus.PendingApproval)
                {
                    if (f.RequesterCharacterId == client.Character.CharacterId)
                    {
                        apprList.Add(CharacterToCommunityInfo(otherCharacter));
                    }
                    else if (f.RequestedCharacterId == client.Character.CharacterId)
                    {
                        applList.Add(CharacterToCommunityInfo(otherCharacter));
                    }
                }
            }
            
            var result = new S2CFriendGetFriendListRes()
            {
                FriendInfoList = fList,
                ApplyingCharacterList = applList,
                ApprovingCharacterList = apprList,
                Result = 0,
                Error = 0,
                    
            };
            

            client.Send(result);
        }
        
        private CDataCommunityCharacterBaseInfo CharacterToCommunityInfo(Character c)
        {
            return new CDataCommunityCharacterBaseInfo()
            {
                CharacterId = c.CharacterId,
                CharacterName = new CDataCharacterName()
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName
                },
                ClanName = "", // TODO get clan

            };

        }
        
        private CDataFriendInfo CharacterToFriendInfo(Character c)
        {
            return new CDataFriendInfo()
            {
                CharacterListElement = new CDataCharacterListElement()
                {
                    OnlineStatus = c.OnlineStatus,
                    MatchingProfile = c.MatchingProfile.Comment,
                    ServerId = c.Server.Id,
                    CommunityCharacterBaseInfo = new CDataCommunityCharacterBaseInfo()
                    {
                        CharacterId = c.CharacterId,
                        CharacterName = new CDataCharacterName()
                        {
                            FirstName = c.FirstName,
                            LastName = c.LastName
                        },
                        ClanName = "" // TODO
                    },
                    CurrentJobBaseInfo = new CDataJobBaseInfo()
                    {
                        Job = c.Job,
                        Level = 10 // TODO
                    },
                    EntryJobBaseInfo = new CDataJobBaseInfo()
                    {
                        // TODO
                        Job = JobId.Hunter,
                        Level = 20
                    }
                }
            };

        }
    }
}
