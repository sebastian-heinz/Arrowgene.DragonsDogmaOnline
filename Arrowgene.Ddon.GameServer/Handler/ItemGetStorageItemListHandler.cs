using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemGetStorageItemListHandler : GameStructurePacketHandler<C2SItemGetStorageItemListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemGetStorageItemListHandler));


        public ItemGetStorageItemListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SItemGetStorageItemListReq> packet)
        {
            S2CItemGetStorageItemListRes res = new S2CItemGetStorageItemListRes()
            {
                ItemList = packet.Structure.StorageList
                    .SelectMany(cDataCommonU8 => client.Character.Storage.getStorageAsCDataItemList((StorageType) cDataCommonU8.Value))
                    .ToList()
            };
            client.Send(res);
        }
    }
}
