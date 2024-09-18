using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardEntryBoardListHandler : GameRequestPacketHandler<C2SEntryBoardEntryBoardListReq, S2CEntryBoardEntryBoardListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardEntryBoardListHandler));


        public EntryBoardEntryBoardListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEntryBoardEntryBoardListRes Handle(GameClient client, C2SEntryBoardEntryBoardListReq request)
        {
            // var result = new S2CEntryBoardEntryBoardListRes.Serializer().Read(GameFull.Dump_709.AsBuffer());

            var result = new S2CEntryBoardEntryBoardListRes();
            foreach (var boardId in request.BoardIdList.Select(x => x.Value).ToList())
            {
                var quest = QuestManager.GetQuestByBoardId(boardId);
                if (quest != null)
                {
                    var contentParams = new CDataEntryBoardListParam()
                    {
                        BoardId = boardId,
                        SortieMin = (ushort) quest.MissionParams.MinimumMembers,
                        NoPartyMembers = (ushort) quest.MissionParams.MaximumMembers,
                        TimeOut = BoardManager.PARTY_BOARD_TIMEOUT,
                    };
                    result.EntryList.Add(contentParams);
                }
            }

            return result;
        }
    }
}
