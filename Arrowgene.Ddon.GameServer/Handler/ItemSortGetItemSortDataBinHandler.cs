using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemSortGetItemSortDataBinHandler : PacketHandler<GameClient>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemSortGetItemSortDataBinHandler));


        public ItemSortGetItemSortDataBinHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketId Id => PacketId.C2S_ITEM_SORT_GET_ITEM_SORTDATA_BIN_REQ;

        public override void Handle(GameClient client, IPacket packet)
        {
            client.Send(EntitySerializer.Get<S2CItemSortGetItemSortdataBinNtc>().Read(InGameDump.data_Dump_37));
            client.Send(EntitySerializer.Get<S2CItemSortGetItemSortdataBinRes>().Read(InGameDump.data_Dump_38));
        }
    }
}
