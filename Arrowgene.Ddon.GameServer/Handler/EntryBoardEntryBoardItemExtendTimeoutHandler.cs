using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardEntryBoardItemExtendTimeoutHandler : GameRequestPacketHandler<C2SEntryBoardEntryBoardExtendTimeoutReq, S2CEntryBoardEntryBoardExtendTimeoutRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardEntryBoardItemExtendTimeoutHandler));

        public EntryBoardEntryBoardItemExtendTimeoutHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEntryBoardEntryBoardExtendTimeoutRes Handle(GameClient client, C2SEntryBoardEntryBoardExtendTimeoutReq request)
        {
            // var pcap = new S2CEntryBoardEntryBoardItemForceStartRes.Serializer().Read(GameFull.Dump_711.AsBuffer());
            var data = Server.BoardManager.GetGroupDataForCharacter(client.Character);

            if (!Server.BoardManager.ExtendReadyUpTimer(data.EntryItem.Id))
            {
                return new S2CEntryBoardEntryBoardExtendTimeoutRes()
                {
                    Error = (uint) ErrorCode.ERROR_CODE_TIMER_INTERNAL_ERROR
                };
            }

            foreach (var characterId in data.Members)
            {
                var memberClient = Server.ClientLookup.GetClientByCharacterId(characterId);
                memberClient.Send(new S2CEntryBoardEntryBoardItemReadyNtc()
                {
                    MaxMember = data.EntryItem.Param.MaxEntryNum,
                    TimeOut = Server.BoardManager.GetTimeLeftToReadyUp(data.EntryItem.Id),
                });
                memberClient.Send(new S2CEntryBoardItemTimeoutTimerNtc() { TimeOut = Server.BoardManager.GetTimeLeftToReadyUp(data.EntryItem.Id) });
            }

            return new S2CEntryBoardEntryBoardExtendTimeoutRes()
            {
                TimeOut = Server.BoardManager.GetTimeLeftToReadyUp(data.EntryItem.Id)
            };
        }
    }
}
