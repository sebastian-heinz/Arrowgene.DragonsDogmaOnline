using System;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemMoveItemHandler : GameStructurePacketHandler<C2SItemMoveItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemGetStorageItemListHandler));

        private static readonly StorageType[] ItemBagStorageTypes = new StorageType[] { StorageType.ItemBagConsumable, StorageType.ItemBagMaterial, StorageType.ItemBagEquipment, StorageType.ItemBagJob };

        public ItemMoveItemHandler(DdonGameServer server) : base(server)
        {
        }
        public override void Handle(GameClient client, StructurePacket<C2SItemMoveItemReq> packet)
        {
            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc();
            ntc.UpdateType = DetermineUpdateType(packet.Structure.SourceGameStorageType);

            foreach (CDataMoveItemUIDFromTo itemFromTo in packet.Structure.ItemUIDList)
            {
                var tuple = client.Character.Storage.getStorage(itemFromTo.SrcStorageType).Items
                    .Select((item, index) => new { item, index })
                    .Where(tuple => itemFromTo.ItemUId == tuple.item?.Item1.UId)
                    .First();
                Item item = tuple.item.Item1;
                ushort srcSlotNo = (ushort) (tuple.index+1);
                ushort dstSlotNo = itemFromTo.SlotNo;
                uint oldSrcItemNum = tuple.item.Item2;
                uint newSrcItemNum;
                uint oldDstItemNum;
                uint newDstItemNum;

                if(dstSlotNo == 0)
                {
                    // Check if there's already of the item in the dst storage
                    var itemInDstSlot = client.Character.Storage.getStorage(itemFromTo.DstStorageType).Items
                        .Select((item, index) => new { item, index })
                        .Where(tuple => itemFromTo.ItemUId == tuple.item?.Item1.UId)
                        .FirstOrDefault();

                    if(itemInDstSlot == null)
                    {
                        oldDstItemNum = 0;
                        dstSlotNo = client.Character.Storage.addStorageItem(item, oldDstItemNum, itemFromTo.DstStorageType);
                    }
                    else
                    {
                        oldDstItemNum = itemInDstSlot.item.Item2;
                        dstSlotNo = (ushort) (itemInDstSlot.index+1);
                    }
                }
                else
                {
                    Tuple<Item, uint> itemInDstSlot = client.Character.Storage.getStorageItem(itemFromTo.DstStorageType, dstSlotNo);
                    oldDstItemNum = itemInDstSlot.Item2;
                }

                ClientItemInfo clientItemInfo = ClientItemInfo.GetInfoForItemId(Server.AssetRepository.ClientItemInfos, item.ItemId);
                uint sanitizedItemFromToNum = Math.Min(itemFromTo.Num, oldSrcItemNum);
                newDstItemNum = oldDstItemNum + sanitizedItemFromToNum;
                if(ItemBagStorageTypes.Contains(itemFromTo.DstStorageType))
                {
                    // Limit items to the item bag stack limit when moving to the item bag
                    newDstItemNum = Math.Min(clientItemInfo.StackLimit, oldDstItemNum + sanitizedItemFromToNum);
                }
                uint movedItemNum = newDstItemNum - oldDstItemNum;
                client.Character.Storage.setStorageItem(item, newDstItemNum, itemFromTo.DstStorageType, dstSlotNo);
                Server.Database.ReplaceStorageItem(client.Character.Id, itemFromTo.DstStorageType, dstSlotNo, item.UId, newDstItemNum);

                CDataItemUpdateResult dstUpdateItem = new CDataItemUpdateResult();
                dstUpdateItem.ItemList.ItemUId = item.UId;
                dstUpdateItem.ItemList.ItemId = item.ItemId;
                dstUpdateItem.ItemList.ItemNum = newDstItemNum;
                dstUpdateItem.ItemList.Unk3 = item.Unk3;
                dstUpdateItem.ItemList.StorageType = itemFromTo.DstStorageType;
                dstUpdateItem.ItemList.SlotNo = dstSlotNo;
                dstUpdateItem.ItemList.Color = item.Color; // ?
                dstUpdateItem.ItemList.PlusValue = item.PlusValue; // ?
                dstUpdateItem.ItemList.Bind = false;
                dstUpdateItem.ItemList.EquipPoint = 0;
                dstUpdateItem.ItemList.EquipCharacterID = 0;
                dstUpdateItem.ItemList.EquipPawnID = 0;
                dstUpdateItem.ItemList.WeaponCrestDataList = item.WeaponCrestDataList;
                dstUpdateItem.ItemList.ArmorCrestDataList = item.ArmorCrestDataList;
                dstUpdateItem.ItemList.EquipElementParamList = item.EquipElementParamList;
                dstUpdateItem.UpdateItemNum = (int) movedItemNum;
                ntc.UpdateItemList.Add(dstUpdateItem);

                // Update item in src storage
                // TODO: Handle swapping items
                newSrcItemNum = oldSrcItemNum - movedItemNum;
                if(newSrcItemNum == 0)
                {
                    client.Character.Storage.setStorageItem(null, 0, itemFromTo.SrcStorageType, srcSlotNo);
                    Server.Database.DeleteStorageItem(client.Character.Id, itemFromTo.SrcStorageType, srcSlotNo);
                }
                else
                {
                    client.Character.Storage.setStorageItem(item, newSrcItemNum, itemFromTo.SrcStorageType, srcSlotNo);
                    Server.Database.ReplaceStorageItem(client.Character.Id, itemFromTo.SrcStorageType, srcSlotNo, item.UId, newSrcItemNum);
                }
                CDataItemUpdateResult srcUpdateItem = new CDataItemUpdateResult();
                srcUpdateItem.ItemList.ItemUId = item.UId;
                srcUpdateItem.ItemList.ItemId = item.ItemId;
                srcUpdateItem.ItemList.ItemNum = newSrcItemNum;
                srcUpdateItem.ItemList.Unk3 = item.Unk3;
                srcUpdateItem.ItemList.StorageType = itemFromTo.SrcStorageType;
                srcUpdateItem.ItemList.SlotNo = srcSlotNo;
                srcUpdateItem.ItemList.Color = item.Color; // ?
                srcUpdateItem.ItemList.PlusValue = item.PlusValue; // ?
                srcUpdateItem.ItemList.Bind = false;
                srcUpdateItem.ItemList.EquipPoint = 0;
                srcUpdateItem.ItemList.EquipCharacterID = 0;
                srcUpdateItem.ItemList.EquipPawnID = 0;
                srcUpdateItem.ItemList.WeaponCrestDataList = item.WeaponCrestDataList;
                srcUpdateItem.ItemList.ArmorCrestDataList = item.ArmorCrestDataList;
                srcUpdateItem.ItemList.EquipElementParamList = item.EquipElementParamList;
                srcUpdateItem.UpdateItemNum = (int) -movedItemNum;
                ntc.UpdateItemList.Add(srcUpdateItem);
            }

            client.Send(ntc);
            client.Send(new S2CItemMoveItemRes());
        }

        // Taken from sItemManager::moveItemsFunc (0xB9F867 in the PC Dump)
        // TODO: Cleanup
        private ushort DetermineUpdateType(byte sourceGameStorageType)
        {
            switch ( sourceGameStorageType )
            {
                case 1:
                    return 49;
                case 7:
                    return 22;
                case 19:
                    return 8;
                case 20:
                    return 9;
                default:
                    return 0;
            }
        }
    }
}