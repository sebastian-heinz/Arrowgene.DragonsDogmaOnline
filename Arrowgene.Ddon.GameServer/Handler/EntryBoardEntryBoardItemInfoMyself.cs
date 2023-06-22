using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class EntryBoardEntryBoardItemInfoMyself : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EntryBoardEntryBoardItemInfoMyself));


        public EntryBoardEntryBoardItemInfoMyself(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_ENTRY_BOARD_ENTRY_BOARD_ITEM_INFO_MYSELF_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            client.Send(GameFull.Dump_712);
        }
    }
}