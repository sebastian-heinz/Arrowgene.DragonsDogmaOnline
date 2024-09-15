using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardEntryBoardItemInfoHandler : GameRequestPacketHandler<C2SEntryBoardEntryBoardItemReq, S2CEntryBoardEntryBoardItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardEntryBoardItemInfoHandler));

        public EntryBoardEntryBoardItemInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CEntryBoardEntryBoardItemRes Handle(GameClient client, C2SEntryBoardEntryBoardItemReq request)
        {
            var data = Server.BoardManager.GetGroupData(request.EntryId);
            return new S2CEntryBoardEntryBoardItemRes()
            {
                BoardId = request.BoardId,
                EntryItemData = data.EntryItem
            };
        }
    }
}
