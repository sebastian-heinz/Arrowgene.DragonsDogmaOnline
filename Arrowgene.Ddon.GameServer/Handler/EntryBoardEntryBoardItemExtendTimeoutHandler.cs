using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardEntryBoardItemExtendTimeoutHandler : GameRequestPacketHandler<C2SEntryBoardEntryBoardItemExtendTimeoutReq, S2CEntryBoardEntryBoardItemExtendTimeoutRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardEntryBoardItemExtendTimeoutHandler));

        public EntryBoardEntryBoardItemExtendTimeoutHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEntryBoardEntryBoardItemExtendTimeoutRes Handle(GameClient client, C2SEntryBoardEntryBoardItemExtendTimeoutReq request)
        {
            // var pcap = new S2CEntryBoardEntryBoardItemForceStartRes.Serializer().Read(GameFull.Dump_711.AsBuffer());
            var data = Server.BoardManager.GetGroupDataForCharacter(client.Character);

            if (!Server.BoardManager.ExtendReadyUpTimer(data.EntryItem.Id))
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_TIMER_INTERNAL_ERROR);
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

            return new S2CEntryBoardEntryBoardItemExtendTimeoutRes()
            {
                TimeOut = Server.BoardManager.GetTimeLeftToReadyUp(data.EntryItem.Id)
            };
        }
    }
}
