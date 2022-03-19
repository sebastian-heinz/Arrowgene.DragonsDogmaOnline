using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemSortSetItemSortdataBinHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemSortSetItemSortdataBinHandler));


        public ItemSortSetItemSortdataBinHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_ITEM_SORT_SET_ITEM_SORTDATA_BIN_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            client.Send(SelectedDump.AntiDcSortdataBin);
        }
    }
}
