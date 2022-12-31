using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            ntc.UpdateType = 8;

            foreach (CDataMoveItemUIDFromTo itemFromTo in packet.Structure.ItemUIDList)
            {
                ushort dstSlotNo = itemFromTo.SlotNo;
                if(dstSlotNo == 0) {
                    for (dstSlotNo = 0; dstSlotNo < client.Character.Items[itemFromTo.DstStorageType].Count; dstSlotNo++)
                    {
                        if(client.Character.Items[itemFromTo.DstStorageType][dstSlotNo] == null)
                        {
                            break;
                        }
                    }
                    dstSlotNo++;
                }

                // TODO: Move only item.Num and not the entire thing
                var tuple = client.Character.Items[itemFromTo.SrcStorageType]
                    .Select((item, index) => new { item, index })
                    .Where(tuple => itemFromTo.ItemUId == tuple.item?.EquipItemUId)
                    .Single();
                EquipItem item = tuple.item;
                ushort srcSlotNo = (ushort) (tuple.index+1);

                // Update item
                CDataItemUpdateResult updateItem = new CDataItemUpdateResult();
                updateItem.ItemList.ItemUId = item.EquipItemUId;
                updateItem.ItemList.ItemId = item.ItemId;
                updateItem.ItemList.ItemNum = 10-itemFromTo.Num; // TODO: yknow
                updateItem.ItemList.Unk3 = item.Unk0;
                updateItem.ItemList.StorageType = item.EquipType;
                updateItem.ItemList.SlotNo = srcSlotNo;
                updateItem.ItemList.Unk6 = item.Color; // ?
                updateItem.ItemList.Unk7 = item.PlusValue; // ?
                updateItem.ItemList.Bind = false;
                updateItem.ItemList.Unk9 = 0;
                updateItem.ItemList.Unk10 = 0;
                updateItem.ItemList.Unk11 = 0;
                updateItem.ItemList.WeaponCrestDataList = item.WeaponCrestDataList;
                updateItem.ItemList.ArmorCrestDataList = item.ArmorCrestDataList;
                updateItem.ItemList.EquipElementParamList = item.EquipElementParamList;
                updateItem.UpdateItemNum = (int) -itemFromTo.Num;
                ntc.UpdateItemList.Add(updateItem);

                client.Character.Items[itemFromTo.SrcStorageType][srcSlotNo] = null;
                item.EquipType = (byte) itemFromTo.DstStorageType;
                item.EquipSlot = dstSlotNo;
                client.Character.Items[itemFromTo.DstStorageType][dstSlotNo] = item;

                CDataItemUpdateResult updateItem2 = new CDataItemUpdateResult();
                updateItem2.ItemList.ItemUId = item.EquipItemUId;
                updateItem2.ItemList.ItemId = item.ItemId;
                updateItem2.ItemList.ItemNum = itemFromTo.Num;
                updateItem2.ItemList.Unk3 = item.Unk0;
                updateItem2.ItemList.StorageType = item.EquipType;
                updateItem2.ItemList.SlotNo = dstSlotNo;
                updateItem2.ItemList.Unk6 = item.Color; // ?
                updateItem2.ItemList.Unk7 = item.PlusValue; // ?
                updateItem2.ItemList.Bind = false;
                updateItem2.ItemList.Unk9 = 0;
                updateItem2.ItemList.Unk10 = 0;
                updateItem2.ItemList.Unk11 = 0;
                updateItem2.ItemList.WeaponCrestDataList = item.WeaponCrestDataList;
                updateItem2.ItemList.ArmorCrestDataList = item.ArmorCrestDataList;
                updateItem2.ItemList.EquipElementParamList = item.EquipElementParamList;
                updateItem2.UpdateItemNum = (int) itemFromTo.Num;
                ntc.UpdateItemList.Add(updateItem2);
            }

            client.Send(ntc);
            client.Send(new S2CItemMoveItemRes());
        }
    }
}