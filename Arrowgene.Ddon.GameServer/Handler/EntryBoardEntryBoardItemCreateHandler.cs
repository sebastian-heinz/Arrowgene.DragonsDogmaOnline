using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.GameServer.Utils;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardEntryBoardItemCreateHandler : GameRequestPacketHandler<C2SEntryBoardEntryBoardItemCreateReq, S2CEntryBoardEntryBoardItemCreateRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardEntryBoardItemCreateHandler));

        public EntryBoardEntryBoardItemCreateHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEntryBoardEntryBoardItemCreateRes Handle(GameClient client, C2SEntryBoardEntryBoardItemCreateReq request)
        {
            // var pcap = new S2CEntryBoardEntryBoardItemCreateRes.Serializer().Read(GameFull.Dump_710.AsBuffer())

            var data = Server.BoardManager.CreateNewGroup(request.BoardId, request.CreateParam, request.Password, client.Character);

            if (BoardManager.BoardIdIsExm(request.BoardId))
            {
                // Override some defaults using JSON config
                var quest = QuestManager.GetQuestByBoardId(request.BoardId);
                data.EntryItem.Param.MinEntryNum = (ushort)quest.MissionParams.MinimumMembers;
                data.EntryItem.Param.MaxEntryNum = (ushort)quest.MissionParams.MaximumMembers;
            }
            else if (BoardManager.BoardIdIsRecruitmentCategory(request.BoardId))
            {
                uint recruitmentCategory = BoardManager.RecruitmentCategoryFromBoardId(request.BoardId);
                var recruitmentData = Server.AssetRepository.RecruitmentBoardCategoryAsset.RecruitmentBoardCategories[recruitmentCategory];
                data.EntryItem.Param.MinEntryNum = recruitmentData.PartyMin;
                data.EntryItem.Param.MaxEntryNum = recruitmentData.PartyMax;
            }

            data.EntryItem.PartyLeaderCharacterId = data.PartyLeaderCharacterId;
            data.EntryItem.TimeOut = BoardManager.PARTY_BOARD_TIMEOUT;

            var member = new CDataEntryMemberData()
            {
                EntryFlag = true,
                Id = 1,
            };
            GameStructure.CDataCharacterListElement(member.CharacterListElement, client.Character);
            data.EntryItem.EntryMemberList.Add(member);

            for (int i = 2; i < data.EntryItem.Param.MaxEntryNum + 1; i++)
            {
                data.EntryItem.EntryMemberList.Add(new CDataEntryMemberData()
                {
                    EntryFlag = false,
                    Id = (ushort) i
                });
            }

            S2CEntryBoardEntryBoardItemChangeMemberNtc ntc = new S2CEntryBoardEntryBoardItemChangeMemberNtc()
            {
                EntryFlag = true,
                MemberData = member,
            };
            client.Send(ntc);

            Server.BoardManager.StartRecruitmentTimer(data.EntryItem.Id, BoardManager.PARTY_BOARD_TIMEOUT);
            Server.CharacterManager.UpdateOnlineStatus(client, client.Character, OnlineStatus.EntryBoard);

            return new S2CEntryBoardEntryBoardItemCreateRes()
            {
                BoardId = request.BoardId,
                EntryItem = data.EntryItem
            };
        }
    }
}
