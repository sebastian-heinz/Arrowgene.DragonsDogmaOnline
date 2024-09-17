using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Diagnostics.Metrics;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardEntryRecreateHandler : GameRequestPacketHandler<C2SEntryBoardEntryBoardItemRecreateReq, S2CEntryBoardEntryBoardItemRecreateRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardEntryRecreateHandler));

        public EntryBoardEntryRecreateHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEntryBoardEntryBoardItemRecreateRes Handle(GameClient client, C2SEntryBoardEntryBoardItemRecreateReq request)
        {
            var data = Server.BoardManager.RecreateGroup(client.Character);

            data.EntryItem.TimeOut = BoardManager.PARTY_BOARD_TIMEOUT;
            foreach (var characterId in data.Members)
            {
                var memberClient = Server.ClientLookup.GetClientByCharacterId(characterId);
                memberClient.Send(new S2CEntryBoardEntryBoardItemRecreateNtc()
                {
                    BoardId = data.BoardId,
                    EntryItem = data.EntryItem
                });

                memberClient.Send(new S2CPartyPartyBreakupNtc());
            }

            return new S2CEntryBoardEntryBoardItemRecreateRes()
            {
                BoardId = data.BoardId,
                EntryItem = data.EntryItem
            };
        }
    }
}
