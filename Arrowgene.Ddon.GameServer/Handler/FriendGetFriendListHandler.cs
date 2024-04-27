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

        public override void Handle(GameClient client, IPacket packet)
        {
            // client.Send(InGameDump.Dump_60);
            List<CDataFriendInfo> fList = new List<CDataFriendInfo>();
            List<CDataCommunityCharacterBaseInfo> applList = new List<CDataCommunityCharacterBaseInfo>();
            List<CDataCommunityCharacterBaseInfo> apprList = new List<CDataCommunityCharacterBaseInfo>();
            List<ContactListEntity> friends = Database.SelectFriends(client.Character.CharacterId);
            
            foreach (var f in friends)
            {
                if (f.Type != ContactListType.FriendList) continue;
                Character otherCharacter =
                    Database.SelectCharacter(f.GetOtherCharacterId(client.Character.CharacterId));
                
                if (f.Status == ContactListStatus.Accepted)
                {
                    fList.Add(ContactListEntity.CharacterToFriend(otherCharacter, f.Id, f.IsFavoriteForCharacter(client.Character.CharacterId)));
                } 
                else if (f.Status == ContactListStatus.PendingApproval)
                {
                    if (f.RequesterCharacterId == client.Character.CharacterId)
                    {
                        applList.Add(ContactListEntity.CharacterToCommunityInfo(otherCharacter));
                    }
                    else if (f.RequestedCharacterId == client.Character.CharacterId)
                    {
                        apprList.Add(ContactListEntity.CharacterToCommunityInfo(otherCharacter));
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
    }
}
