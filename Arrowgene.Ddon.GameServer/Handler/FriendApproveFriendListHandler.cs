using System;
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
    public class FriendApproveFriendListHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(FriendApproveFriendListHandler));


        public FriendApproveFriendListHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_FRIEND_APPROVE_FRIEND_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            
            var Result = new S2CFriendApproveFriendRes()
            {
                FriendInfo = new CDataFriendInfo()
                {
                    IsFavorite = false,
                    PendingStatus = 0x00,
                    UnFriendNo = 1,
                    CharacterListElement = new CDataCharacterListElement()
                    {
                        OnlineStatus = OnlineStatus.Online,
                        MatchingProfile = "WOW!",
                        ServerId = UInt16.MinValue,
                        CommunityCharacterBaseInfo = new CDataCommunityCharacterBaseInfo()
                        {
                            CharacterId = 1,
                            CharacterName = new CDataCharacterName()
                            {
                                FirstName = "Your",
                                LastName = "Sister"
                            },
                            ClanName = "Clan Name"
                        },
                        CurrentJobBaseInfo = new CDataJobBaseInfo()
                        {
                            Job = JobId.Alchemist,
                            Level = 10
                        },
                        EntryJobBaseInfo = new CDataJobBaseInfo()
                        {
                            Job = JobId.Hunter,
                            Level = 20
                        }
                    }
                    
                },
                Result = 0,
                Error = 0,
                    
            };
            

            client.Send(Result);
        }
    }
}
