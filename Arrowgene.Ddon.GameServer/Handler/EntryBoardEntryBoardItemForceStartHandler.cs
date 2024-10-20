using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardEntryBoardItemForceStartHandler : GameRequestPacketHandler<C2SEntryBoardEntryBoardItemForceStartReq, S2CEntryBoardEntryBoardItemForceStartRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardEntryBoardItemForceStartHandler));

        public EntryBoardEntryBoardItemForceStartHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEntryBoardEntryBoardItemForceStartRes Handle(GameClient client, C2SEntryBoardEntryBoardItemForceStartReq request)
        {
            // var pcap = new S2CEntryBoardEntryBoardItemForceStartRes.Serializer().Read(GameFull.Dump_711.AsBuffer());

            var data = Server.BoardManager.GetGroupDataForCharacter(client.Character);

            Server.BoardManager.CancelRecruitmentTimer(data.EntryItem.Id);
            foreach (var characterId in data.Members)
            {
                var memberClient = Server.ClientLookup.GetClientByCharacterId(characterId);
                // Allows the menu to transition
                var ntc = new S2CEntryBoardEntryBoardItemReadyNtc()
                {
                    MaxMember = data.EntryItem.Param.MaxEntryNum,
                    TimeOut = BoardManager.ENTRY_BOARD_READY_TIMEOUT,
                };
                memberClient.Send(ntc);
                memberClient.Send(new S2CEntryBoardItemTimeoutTimerNtc() {TimeOut = BoardManager.ENTRY_BOARD_READY_TIMEOUT });
            }

            Server.BoardManager.StartReadyUpTimer(data.EntryItem.Id, BoardManager.ENTRY_BOARD_READY_TIMEOUT);

            return new S2CEntryBoardEntryBoardItemForceStartRes();
        }
    }
}
