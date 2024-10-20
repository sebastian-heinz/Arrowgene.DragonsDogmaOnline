using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardEntryBoardItemLeaveHandler : GameRequestPacketHandler<C2SEntryBoardEntryBoardItemLeaveReq, S2CEntryBoardEntryBoardItemLeaveRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardEntryBoardItemLeaveHandler));

        public EntryBoardEntryBoardItemLeaveHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEntryBoardEntryBoardItemLeaveRes Handle(GameClient client, C2SEntryBoardEntryBoardItemLeaveReq request)
        {
            var data = Server.BoardManager.GetGroupDataForCharacter(client.Character);
            if (data.PartyLeaderCharacterId == client.Character.CharacterId)
            {
                Server.BoardManager.RemoveGroup(data.EntryItem.Id);
                foreach (var characterId in data.Members)
                {
                    var memberClient = Server.ClientLookup.GetClientByCharacterId(characterId);
                    if (memberClient != null)
                    {
                        memberClient.Send(new S2CEntryBoardEntryBoardItemLeaveNtc());
                        Server.CharacterManager.UpdateOnlineStatus(client, memberClient.Character, OnlineStatus.Online);
                    }
                }
            }
            else
            {
                CDataEntryMemberData memberData;
                lock (data)
                {
                    memberData = data.EntryItem.EntryMemberList.Where(x => x.CharacterListElement.CommunityCharacterBaseInfo.CharacterId == client.Character.CharacterId).First();
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

                client.Send(new S2CEntryBoardEntryBoardItemLeaveNtc());

                Server.BoardManager.CancelReadyUpTimer(data.EntryItem.Id);
                Server.BoardManager.RemoveCharacterFromGroup(client.Character);
                Server.BoardManager.RestartRecruitment(data.EntryItem.Id);

                Server.CharacterManager.UpdateOnlineStatus(client, client.Character, OnlineStatus.Online);
            }

            return new S2CEntryBoardEntryBoardItemLeaveRes();
        }
    }
}
