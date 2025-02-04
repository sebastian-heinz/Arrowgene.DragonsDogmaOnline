using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemSellItemHandler : GameRequestPacketQueueHandler<C2SItemSellItemReq, S2CItemSellItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemSellItemHandler));
        
        public ItemSellItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SItemSellItemReq request)
        {
            PacketQueue packetQueue = new PacketQueue();
            client.Enqueue(new S2CItemSellItemRes(), packetQueue);

            uint totalAmountToAdd = 0;

            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.ShopItemSell
            };

            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (CDataStorageItemUIDList consumeItem in request.ConsumeItemList)
                {
                    var ntcData = Server.ItemManager.ConsumeItemByUId(Server, client.Character, consumeItem.StorageType, consumeItem.ItemUId, consumeItem.Num, connection);
                    ntc.UpdateItemList.Add(ntcData);

                    uint goldValue = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, ntcData.ItemList.ItemId).Price;
                    uint amountToAdd = goldValue * consumeItem.Num;
                    totalAmountToAdd += amountToAdd;
                }
                CDataUpdateWalletPoint walletUpdate = Server.WalletManager.AddToWallet(client.Character, WalletType.Gold, totalAmountToAdd, 0, connection);
                ntc.UpdateWalletList.Add(walletUpdate);
            });

            client.Enqueue(ntc, packetQueue);

            return packetQueue;
        }
    }
}
