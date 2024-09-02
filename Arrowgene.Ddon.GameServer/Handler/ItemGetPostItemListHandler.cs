using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemGetPostItemListHandler : GameRequestPacketHandler<C2SItemGetPostItemListReq, S2CItemGetPostItemListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemGetPostItemListHandler));

        public ItemGetPostItemListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CItemGetPostItemListRes Handle(GameClient client, C2SItemGetPostItemListReq request)
        {
            S2CItemGetPostItemListRes res = new S2CItemGetPostItemListRes()
            {
                ItemList = client.Character.Storage.GetStorageAsCDataItemList(client.Character, Shared.Model.StorageType.ItemPost)
            };
            return res;
        }
    }
}
