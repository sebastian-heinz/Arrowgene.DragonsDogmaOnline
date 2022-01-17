using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemSortGetItemSortDataBinHandler : PacketHandler<GameClient>
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(ItemSortGetItemSortDataBinHandler));


        public ItemSortGetItemSortDataBinHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_ITEM_SORT_GET_ITEM_SORTDATA_BIN_REQ;

        public override void Handle(GameClient client, Packet packet)
        {
            
            client.Send(InGameDump.Dump_37);
            client.Send(InGameDump.Dump_38);
        }
    }
}
