using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemMoveItemHandler : GameRequestPacketHandler<C2SItemMoveItemReq, S2CItemMoveItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemGetStorageItemListHandler));

        public ItemMoveItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CItemMoveItemRes Handle(GameClient client, C2SItemMoveItemReq request)
        {
            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc();

            if (request.ItemUIDList.Any())
            {
                // Condense all the source stacks into one structure; they may be going to different locations though (e.g. storage box -> item bag)
                // Structure is (Destination, ID) -> Total Count.
                Dictionary<(StorageType, uint), uint> itemCounts = new();
                foreach (CDataMoveItemUIDFromTo itemFromTo in request.ItemUIDList)
                {
                    var storageItem = client.Character.Storage.GetStorage(itemFromTo.SrcStorageType).FindItemByUId(itemFromTo.ItemUId);
                    var itemId = storageItem.Item2.ItemId;
                    itemCounts[(itemFromTo.DstStorageType, itemId)] = itemCounts.GetValueOrDefault((itemFromTo.DstStorageType, itemId)) + itemFromTo.Num;
                }

                // Second pass to convert the previous structure into one of (Destination, Slots)
                Dictionary<StorageType, uint> slotsRequired = new();
                foreach (var condensedItem in itemCounts)
                {
                    StorageType destinationStorageType = condensedItem.Key.Item1;
                    uint slotsPerItem = Server.ItemManager.PredictAddItemSlots(client.Character, destinationStorageType, condensedItem.Key.Item2, condensedItem.Value);
                    slotsRequired[destinationStorageType] = slotsRequired.GetValueOrDefault(destinationStorageType) + slotsPerItem;
                }

                // Third pass to check if each storage has enough room.
                foreach (var check in slotsRequired)
                {
                    uint emptySlots = client.Character.Storage.GetStorage(check.Key).EmptySlots();
                    if (emptySlots < check.Value)
                    {
                        if (ItemManager.ItemBagStorageTypes.Contains(check.Key))
                        {
                            throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_BAG_CAPACITY_OVER);
                        }
                        else if (ItemManager.BoxStorageTypes.Contains(check.Key))
                        {
                            throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_STORAGE_CAPACITY_OVER);
                        }
                        else
                        {
                            throw new ResponseErrorException(ErrorCode.ERROR_CODE_ITEM_INTERNAL_ERROR);
                        }
                    }
                }
            }
            
            ntc.UpdateType = DetermineUpdateType(request.SourceGameStorageType);
            Server.Database.ExecuteInTransaction(connection =>
            {
                foreach (CDataMoveItemUIDFromTo itemFromTo in request.ItemUIDList)
                {
                    ntc.UpdateItemList.AddRange(
                        Server.ItemManager.MoveItem(
                            Server,
                            client.Character,
                            client.Character.Storage.GetStorage(itemFromTo.SrcStorageType),
                            itemFromTo.ItemUId,
                            itemFromTo.Num,
                            client.Character.Storage.GetStorage(itemFromTo.DstStorageType),
                            itemFromTo.SlotNo,
                            connection
                        )
                    );
                }
            });
            
            client.Send(ntc);

            return new S2CItemMoveItemRes();
        }

        // Taken from sItemManager::moveItemsFunc (0xB9F867 in the PC Dump)
        // TODO: Cleanup
        private ItemNoticeType DetermineUpdateType(byte sourceGameStorageType)
        {
            switch ( sourceGameStorageType )
            {
                case 1:
                    return ItemNoticeType.TemporaryItems;
                case 7:
                    return ItemNoticeType.ExStorageItems;
                case 8:
                case 9:
                case 10:
                    return ItemNoticeType.BaggageItems; //Found by binary search, may not be the "correct" one, but it does work.
                case 13:
                    return ItemNoticeType.LoadPostItems;
                case 19:
                    return ItemNoticeType.StoreStorage_items;
                case 20:
                    return ItemNoticeType.LoadStorage_items;
                default:
                    return ItemNoticeType.Default;
            }
        }
    }
}
