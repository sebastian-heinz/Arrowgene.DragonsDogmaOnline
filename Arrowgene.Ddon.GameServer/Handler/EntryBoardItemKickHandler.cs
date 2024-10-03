using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Diagnostics.Metrics;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardItemKickHandler : GameRequestPacketHandler<C2SEntryBoardItemKickReq, S2CEntryBoardItemKickRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardItemKickHandler));

        public EntryBoardItemKickHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEntryBoardItemKickRes Handle(GameClient client, C2SEntryBoardItemKickReq request)
        {
            var data = Server.BoardManager.RecreateGroup(client.Character);

            var kickedMemberClient = Server.ClientLookup.GetClientByCharacterId(request.CharacterId);

            CDataEntryMemberData memberData;
            lock (data)
            {
                memberData = data.EntryItem.EntryMemberList.Where(x => x.CharacterListElement.CommunityCharacterBaseInfo.CharacterId == kickedMemberClient.Character.CharacterId).First();
                memberData.EntryFlag = false;
                memberData.CharacterListElement = new CDataCharacterListElement();
            }

            S2CEntryBoardEntryBoardItemChangeMemberNtc ntc = new S2CEntryBoardEntryBoardItemChangeMemberNtc()
            {
                EntryFlag = false,
                MemberData = memberData,
            };

            foreach (var characterId in data.Members)
            {
                var memberClient = Server.ClientLookup.GetClientByCharacterId(characterId);
                if (memberClient != null)
                {
                    memberClient.Send(ntc);
                }
            }

            kickedMemberClient.Send(new S2CEntryBoardEntryBoardItemLeaveNtc());
            
            Server.BoardManager.RemoveCharacterFromGroup(kickedMemberClient.Character);
            Server.CharacterManager.UpdateOnlineStatus(kickedMemberClient, kickedMemberClient.Character, OnlineStatus.Online);

            return new S2CEntryBoardItemKickRes();
        }
    }
}
