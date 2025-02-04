using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardEntryBoardItemEntryHandler : GameRequestPacketQueueHandler<C2SEntryBoardEntryBoardItemEntryReq, S2CEntryBoardEntryBoardItemEntryRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardEntryBoardItemEntryHandler));

        private DdonGameServer _Server;

        public EntryBoardEntryBoardItemEntryHandler(DdonGameServer server) : base(server)
        {
            _Server = server;
        }

        public override PacketQueue Handle(GameClient client, C2SEntryBoardEntryBoardItemEntryReq request)
        {
            PacketQueue queue = new();
            var data = _Server.BoardManager.GetGroupData(request.EntryId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_ENTRY_BOARD_NO_ITEM);

            if (data.ContentStatus != ContentStatus.Recruiting)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ENTRY_BOARD_ITEM_CREATE_OVER);
            }

            if (!_Server.BoardManager.AddCharacterToGroup(data.EntryItem.Id, client.Character))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_ENTRY_BOARD_NO_SPACE);
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
                var memberClient = _Server.ClientLookup.GetClientByCharacterId(characterId)
                    ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_ENTRY_BOARD_NO_USER);
                memberClient.Enqueue(ntc, queue);
            }

            _Server.CharacterManager.UpdateOnlineStatus(client, client.Character, OnlineStatus.EntryBoard);

            data.EntryItem.TimeOut = (ushort)_Server.BoardManager.GetRecruitmentTimeLeft(data.EntryItem.Id);

            client.Enqueue(new S2CEntryBoardEntryBoardItemEntryRes()
            {
                EntryItem = data.EntryItem
            }, queue);

            // Work after the response to the request was handeled
            if (data.Members.Count == data.EntryItem.Param.MaxEntryNum)
            {
                _Server.BoardManager.CancelRecruitmentTimer(data.EntryItem.Id);
                foreach (var characterId in data.Members)
                {
                    var memberClient = _Server.ClientLookup.GetClientByCharacterId(characterId);
                    // Allows the menu to transition
                    memberClient.Enqueue(new S2CEntryBoardEntryBoardItemReadyNtc()
                    {
                        MaxMember = data.EntryItem.Param.MaxEntryNum,
                        TimeOut = BoardManager.ENTRY_BOARD_READY_TIMEOUT,
                    }, queue);
                    memberClient.Enqueue(new S2CEntryBoardItemTimeoutTimerNtc() 
                    { 
                        TimeOut = BoardManager.ENTRY_BOARD_READY_TIMEOUT 
                    }, queue);
                }

                _Server.BoardManager.StartReadyUpTimer(data.EntryItem.Id, BoardManager.ENTRY_BOARD_READY_TIMEOUT);
            }

            return queue;
        }
    }
}

