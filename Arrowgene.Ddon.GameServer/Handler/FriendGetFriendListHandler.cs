using System.Collections.Generic;
using Arrowgene.Buffers;
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
            CDataFriendInfo friend = new CDataFriendInfo()
            {
                CharacterListElement = new CDataCharacterListElement()
                {
                    CommunityCharacterBaseInfo = new CDataCommunityCharacterBaseInfo()
                    {
                        CharacterId = 1,
                        CharacterName = new CDataCharacterName()
                        {
                            FirstName = "Your",
                            LastName = "Mom"
                        },
                        ClanName = "Land Whales",

                    },
                    OnlineStatus = OnlineStatus.Busy,
                    CurrentJobBaseInfo = new CDataJobBaseInfo()
                    {
                        Job = JobId.Seeker,
                        Level = 10
                    }
                }
            };
            CDataFriendInfo friend2 = new CDataFriendInfo()
            {
                CharacterListElement = new CDataCharacterListElement()
                {
                    CommunityCharacterBaseInfo = new CDataCommunityCharacterBaseInfo()
                    {
                        CharacterId = 1,
                        CharacterName = new CDataCharacterName()
                        {
                            FirstName = "Your",
                            LastName = "Dad"
                        },
                        ClanName = "Land Whales",

                    },
                    OnlineStatus = OnlineStatus.Busy,
                    CurrentJobBaseInfo = new CDataJobBaseInfo()
                    {
                        Job = JobId.Fighter,
                        Level = 15
                    }
                }
            };
            fList.Add(friend);
            fList.Add(friend2);

            CDataCommunityCharacterBaseInfo ApplyingFriend = new CDataCommunityCharacterBaseInfo()
            {
                CharacterId = 1,
                CharacterName = new CDataCharacterName()
                {
                    FirstName = "Your",
                    LastName = "ApplyingFriend"
                },
                ClanName = "Land Whales",

            };
            
            
            CDataCommunityCharacterBaseInfo ApprovingFriend = new CDataCommunityCharacterBaseInfo()
            {
                CharacterId = 1,
                CharacterName = new CDataCharacterName()
                {
                    FirstName = "Your",
                    LastName = "ApprovingFriend"
                },
                ClanName = "Land Whales",

            };
            
            List<CDataCommunityCharacterBaseInfo> applList = new List<CDataCommunityCharacterBaseInfo>();
            List<CDataCommunityCharacterBaseInfo> apprList = new List<CDataCommunityCharacterBaseInfo>();
            // applList.Add(ApplyingFriend);
            // apprList.Add(ApprovingFriend);
            
            var Result = new S2CFriendGetFriendListRes()
            {
                FriendInfoList = fList,
                ApplyingCharacterList = applList,
                ApprovingCharacterList = apprList,
                Result = 0,
                Error = 0,
                    
            };
            

            client.Send(Result);
        }
    }
}
