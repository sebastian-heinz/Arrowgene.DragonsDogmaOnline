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
                    .Single();
                Item item = tuple.item.Item1;
                uint itemNum = tuple.item.Item2;
                uint sanitizedItemFromToNum = Math.Clamp(itemFromTo.Num, 0, itemNum);
                ushort srcSlotNo = (ushort) (tuple.index+1);
                uint srcItemNum = itemNum-sanitizedItemFromToNum;

                // Update item in src storage
                CDataItemUpdateResult updateItem = new CDataItemUpdateResult();
                updateItem.ItemList.ItemUId = item.UId;
                updateItem.ItemList.ItemId = item.ItemId;
                updateItem.ItemList.ItemNum = srcItemNum;
                updateItem.ItemList.Unk3 = item.Unk3;
                updateItem.ItemList.StorageType = (byte) itemFromTo.SrcStorageType;
                updateItem.ItemList.SlotNo = srcSlotNo;
                updateItem.ItemList.Color = item.Color; // ?
                updateItem.ItemList.PlusValue = item.PlusValue; // ?
                updateItem.ItemList.Bind = false;
                updateItem.ItemList.EquipPoint = 0;
                updateItem.ItemList.EquipCharacterID = 0;
                updateItem.ItemList.EquipPawnID = 0;
                updateItem.ItemList.WeaponCrestDataList = item.WeaponCrestDataList;
                updateItem.ItemList.ArmorCrestDataList = item.ArmorCrestDataList;
                updateItem.ItemList.EquipElementParamList = item.EquipElementParamList;
                updateItem.UpdateItemNum = (int) -itemFromTo.Num;
                ntc.UpdateItemList.Add(updateItem);

                // TODO: Handle swapping items
                if(srcItemNum == 0)
                {
                    client.Character.Storage.setStorageItem(null, 0, itemFromTo.SrcStorageType, srcSlotNo);
                    Server.Database.DeleteStorageItem(client.Character.Id, itemFromTo.SrcStorageType, srcSlotNo);
                }
                else
                {
                    client.Character.Storage.setStorageItem(item, srcItemNum, itemFromTo.SrcStorageType, srcSlotNo);
                    Server.Database.ReplaceStorageItem(client.Character.Id, itemFromTo.SrcStorageType, srcSlotNo, item.UId, srcItemNum);
                }

                ushort dstSlotNo = itemFromTo.SlotNo;
                uint dstItemNum = sanitizedItemFromToNum;
                if(dstSlotNo == 0)
                {
                    // Check if there's already of the item in the dst storage
                    var itemInDstSlot = client.Character.Storage.getStorage(itemFromTo.DstStorageType).Items
                        .Select((item, index) => new { item, index })
                        .Where(tuple => itemFromTo.ItemUId == tuple.item?.Item1.UId)
                        .SingleOrDefault();

                    if(itemInDstSlot == null)
                    {
                        dstSlotNo = client.Character.Storage.addStorageItem(item, dstItemNum, itemFromTo.DstStorageType);
                    }
                    else
                    {
                        dstSlotNo = (ushort) (itemInDstSlot.index+1);
                        dstItemNum += itemInDstSlot.item.Item2;
                    }
                }
                else
                {
                    Tuple<Item, uint> itemInDstSlot = client.Character.Storage.getStorageItem(itemFromTo.DstStorageType, dstSlotNo);
                    dstItemNum += itemInDstSlot.Item2;
                }
                client.Character.Storage.setStorageItem(item, dstItemNum, itemFromTo.DstStorageType, dstSlotNo);
                Server.Database.ReplaceStorageItem(client.Character.Id, itemFromTo.DstStorageType, dstSlotNo, item.UId, dstItemNum);

                CDataItemUpdateResult updateItem2 = new CDataItemUpdateResult();
                updateItem2.ItemList.ItemUId = item.UId;
                updateItem2.ItemList.ItemId = item.ItemId;
                updateItem2.ItemList.ItemNum = dstItemNum;
                updateItem2.ItemList.Unk3 = item.Unk3;
                updateItem2.ItemList.StorageType = (byte) itemFromTo.DstStorageType;
                updateItem2.ItemList.SlotNo = dstSlotNo;
                updateItem2.ItemList.Color = item.Color; // ?
                updateItem2.ItemList.PlusValue = item.PlusValue; // ?
                updateItem2.ItemList.Bind = false;
                updateItem2.ItemList.EquipPoint = 0;
                updateItem2.ItemList.EquipCharacterID = 0;
                updateItem2.ItemList.EquipPawnID = 0;
                updateItem2.ItemList.WeaponCrestDataList = item.WeaponCrestDataList;
                updateItem2.ItemList.ArmorCrestDataList = item.ArmorCrestDataList;
                updateItem2.ItemList.EquipElementParamList = item.EquipElementParamList;
                updateItem2.UpdateItemNum = (int) itemFromTo.Num;
                ntc.UpdateItemList.Add(updateItem2);
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