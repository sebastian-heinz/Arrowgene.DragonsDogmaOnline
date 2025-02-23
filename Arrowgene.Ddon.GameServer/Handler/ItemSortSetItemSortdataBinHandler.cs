using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemSortSetItemSortDataBinHandler : GameRequestPacketHandler<C2SItemSortSetItemSortDataBinReq, S2CItemSortSetItemSortDataBinRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemSortSetItemSortDataBinHandler));

        public ItemSortSetItemSortDataBinHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CItemSortSetItemSortDataBinRes Handle(GameClient client, C2SItemSortSetItemSortDataBinReq request)
        {
            Storage storage = client.Character.Storage.GetStorage(request.SortData.StorageType);
            storage.SortData = request.SortData.Bin;
            Server.Database.UpdateStorage(client.Character.CharacterId, request.SortData.StorageType, storage);

            return new();
        }
    }
}
