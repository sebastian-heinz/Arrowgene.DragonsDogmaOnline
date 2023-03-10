using System;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemSellItemHandler : GameStructurePacketHandler<C2SItemSellItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemSellItemHandler));
        
        public ItemSellItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SItemSellItemReq> req)
        {
            client.Send(new S2CItemSellItemRes());

            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc();
            ntc.UpdateType = 267;
            foreach (CDataStorageItemUIDList consumeItem in req.Structure.ConsumeItemList)
            {
                var tuple = client.Character.Storage.getStorage(consumeItem.StorageType).Items
                    .Select((item, index) => new {item = item, slot = (ushort) (index+1)})
                    .Where(tuple => tuple.item?.Item1.UId == consumeItem.ItemUId)
                    .First();
                Item item = tuple.item.Item1;
                ushort itemSlot = tuple.slot;
                uint oldItemNum = tuple.item.Item2;
                uint newItemNum = Math.Max(0, oldItemNum - consumeItem.Num);

                CDataItemUpdateResult ntcData0 = new CDataItemUpdateResult();
                ntcData0.ItemList.ItemUId = item.UId;
                ntcData0.ItemList.ItemId = item.ItemId;
                ntcData0.ItemList.ItemNum = newItemNum;
                ntcData0.ItemList.Unk3 = item.Unk3;
                ntcData0.ItemList.StorageType = consumeItem.StorageType;
                ntcData0.ItemList.SlotNo = itemSlot;
                ntcData0.ItemList.Color = item.Color;
                ntcData0.ItemList.PlusValue = item.PlusValue;
                ntcData0.ItemList.Bind = false;
                ntcData0.ItemList.EquipPoint = 0;
                ntcData0.ItemList.EquipCharacterID = 0;
                ntcData0.ItemList.EquipPawnID = 0;
                ntcData0.ItemList.WeaponCrestDataList = item.WeaponCrestDataList;
                ntcData0.ItemList.ArmorCrestDataList = item.ArmorCrestDataList;
                ntcData0.ItemList.EquipElementParamList = item.EquipElementParamList;
                ntcData0.UpdateItemNum = -((int)consumeItem.Num);

                // TODO: Add Gold to ntc.UpdateWallet and DB

                ntc.UpdateItemList.Add(ntcData0);

                if(newItemNum == 0)
                {
                    // Delete item when ItemNum reaches 0 to free up the slot
                    client.Character.Storage.setStorageItem(null, 0, consumeItem.StorageType, itemSlot);
                    Server.Database.DeleteStorageItem(client.Character.Id, consumeItem.StorageType, itemSlot);
                }
                else
                {
                    client.Character.Storage.setStorageItem(item, newItemNum, consumeItem.StorageType, itemSlot);
                    Server.Database.ReplaceStorageItem(client.Character.Id, consumeItem.StorageType, itemSlot, item.UId, newItemNum);
                }

                client.Send(ntc);
            }
        }
    }
}