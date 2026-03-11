using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

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
                    List<StorageType> targetStorage = [];
                    if (consumeItem.StorageType == StorageType.ReceiveInItemBagCraft)
                    {
                        targetStorage = ItemManager.ItemBagStorageTypes;
                    }
                    else if (consumeItem.StorageType == StorageType.ReceiveInStorageCraft)
                    {
                        targetStorage = ItemManager.BoxStorageTypes;
                    }
                    else
                    {
                        targetStorage = [consumeItem.StorageType];                    
                    }

                    var ntcData = Server.ItemManager.ConsumeItemByUIdFromMultipleStorages(Server, client.Character, targetStorage, consumeItem.ItemUId, consumeItem.Num, connection);
                    ntc.UpdateItemList.AddRange(ntcData);

                    uint goldValue = Server.AssetRepository.ClientItemInfos[ntcData.First().ItemList.ItemId].Price;
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
