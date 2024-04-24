using System;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class FriendApplyFriendListHandler :  GameStructurePacketHandler<C2SFriendApplyFriendReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(FriendApplyFriendListHandler));


        public FriendApplyFriendListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SFriendApplyFriendReq> packet)
        {
            Character requestedChar = Server.Database.SelectCharacter(packet.Structure.CharacterId);
            if (requestedChar == null)
            {
                var res = new S2CFriendApplyFriendRes();
                Logger.Error(client, $"not found CharacterId:{packet.Structure.CharacterId} for friend invitation");
                res.Error = (uint)ErrorCode.ERROR_CODE_FAIL;
                client.Send(res);
                return;
            }

            CDataFriendInfo requester = CharacterToFriend(client.Character);
            CDataFriendInfo requested = CharacterToFriend(requestedChar);

            GameClient requestedClient = Server.ClientLookup.GetClientByCharacterId(packet.Structure.CharacterId);
            if (requestedClient != null)
            {
                var ntc = new S2CFriendApplyFriendNtc()
                {
                    FriendInfo = requester
                };
                requestedClient.Send(ntc);
            }

            
            var Result = new S2CFriendApplyFriendRes()
            {
                FriendInfo = requested,
                Result = 0,
                Error = 0,
                    
            };
            
            client.Send(Result);
        }

        private CDataFriendInfo CharacterToFriend(Character c)
        {
            // TODO share this code
            return new CDataFriendInfo()
            {
                IsFavorite = false,
                PendingStatus = 0x00, // TODO
                UnFriendNo = 7, // TODO
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
                        Level = 10
                    },
                    EntryJobBaseInfo = new CDataJobBaseInfo()
                    { // TODO
                        Job = JobId.Hunter,
                        Level = 20
                    }
                }

            };
            
        }
    }
}
