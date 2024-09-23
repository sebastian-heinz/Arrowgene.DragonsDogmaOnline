using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardEntryBoardItemEntryHandler : GameRequestPacketHandler<C2SEntryBoardEntryBoardItemEntryReq, S2CEntryBoardEntryBoardItemEntryRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardEntryBoardItemEntryHandler));

        public EntryBoardEntryBoardItemEntryHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEntryBoardEntryBoardItemEntryRes Handle(GameClient client, C2SEntryBoardEntryBoardItemEntryReq request)
        {
            var data = Server.BoardManager.GetGroupData(request.EntryId);
            if (data == null)
            {
                return new S2CEntryBoardEntryBoardItemEntryRes()
                {
                    Error = (uint)ErrorCode.ERROR_CODE_ENTRY_BOARD_NO_ITEM
                };
            }

            if (data.ContentInProgress)
            {
                return new S2CEntryBoardEntryBoardItemEntryRes()
                {
                    Error = (uint)ErrorCode.ERROR_CODE_ENTRY_BOARD_ITEM_CREATE_OVER
                };
            }

            if (!Server.BoardManager.AddCharacterToGroup(data.EntryItem.Id, client.Character))
            {
                return new S2CEntryBoardEntryBoardItemEntryRes()
                {
                    Error = (uint)ErrorCode.ERROR_CODE_ENTRY_BOARD_NO_SPACE
                };
            }

            CDataEntryMemberData memberData;
            lock (data)
            {
                memberData = data.EntryItem.EntryMemberList.First(x => x.EntryFlag == false);
                memberData.EntryFlag = true;
                GameStructure.CDataCharacterListElement(memberData.CharacterListElement, client.Character);
            }            

            S2CEntryBoardEntryBoardItemChangeMemberNtc ntc = new S2CEntryBoardEntryBoardItemChangeMemberNtc()
            {
                EntryFlag = true,
                MemberData = memberData,
            };

            foreach (var characterId in data.Members)
            {
                var memberClient = Server.ClientLookup.GetClientByCharacterId(characterId);
                memberClient.Send(ntc);
            }

            Server.CharacterManager.UpdateOnlineStatus(client, client.Character, OnlineStatus.EntryBoard);

            data.EntryItem.TimeOut = (ushort)Server.BoardManager.GetRecruitmentTimeLeft(data.EntryItem.Id);

            return new S2CEntryBoardEntryBoardItemEntryRes()
            {
                EntryItem = data.EntryItem
            };
        }
    }
}

