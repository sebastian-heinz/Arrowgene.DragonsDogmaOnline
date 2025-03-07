using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemGetStorageItemListHandler : GameRequestPacketHandler<C2SItemGetStorageItemListReq, S2CItemGetStorageItemListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemGetStorageItemListHandler));


        public ItemGetStorageItemListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CItemGetStorageItemListRes Handle(GameClient client, C2SItemGetStorageItemListReq request)
        {
            S2CItemGetStorageItemListRes res = new S2CItemGetStorageItemListRes()
            {
                ItemList = request.StorageList
                    .SelectMany(cDataCommonU8 => client.Character.Storage.GetStorageAsCDataItemList(client.Character, (StorageType)cDataCommonU8.Value))
                    .ToList()
            };
            return res;
        }
    }
}
