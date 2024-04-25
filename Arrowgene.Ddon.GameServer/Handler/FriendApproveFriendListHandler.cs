using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class FriendApproveFriendListHandler : GameStructurePacketHandler<C2SFriendApproveFriendReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(FriendApproveFriendListHandler));


        public FriendApproveFriendListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SFriendApproveFriendReq> packet)
        {
            if (packet.Structure.IsApproved)
            {
                Database.UpsertContact((int)packet.Structure.CharacterId, (int)client.Character.CharacterId,
                    ContactListStatus.Accepted, ContactListType.FriendList);
            }
            else
            {
                Database.DeleteContact((int)packet.Structure.CharacterId, (int)client.Character.CharacterId);
            }
            
            Character requestingChar = Server.Database.SelectCharacter(packet.Structure.CharacterId);
            var result = new S2CFriendApproveFriendRes()
            {
                FriendInfo = CharacterToFriend(requestingChar),
                Result = 0,
                Error = 0,
                    
            };
            client.Send(result);
            
            GameClient requestingClient = Server.ClientLookup.GetClientByCharacterId(packet.Structure.CharacterId);
            if (requestingClient != null)
            {
                var ntc = new S2CFriendApproveFriendNtc()
                {
                    FriendInfo = CharacterToFriend(client.Character),
                    IsApproved = packet.Structure.IsApproved
                };
                requestingClient.Send(ntc);
            }
        }
        
        private CDataFriendInfo CharacterToFriend(Character c)
        {
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
