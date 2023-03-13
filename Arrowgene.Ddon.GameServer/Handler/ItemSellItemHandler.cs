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

                CDataItemUpdateResult itemUpdate = new CDataItemUpdateResult();
                itemUpdate.ItemList.ItemUId = item.UId;
                itemUpdate.ItemList.ItemId = item.ItemId;
                itemUpdate.ItemList.ItemNum = newItemNum;
                itemUpdate.ItemList.Unk3 = item.Unk3;
                itemUpdate.ItemList.StorageType = consumeItem.StorageType;
                itemUpdate.ItemList.SlotNo = itemSlot;
                itemUpdate.ItemList.Color = item.Color;
                itemUpdate.ItemList.PlusValue = item.PlusValue;
                itemUpdate.ItemList.Bind = false;
                itemUpdate.ItemList.EquipPoint = 0;
                itemUpdate.ItemList.EquipCharacterID = 0;
                itemUpdate.ItemList.EquipPawnID = 0;
                itemUpdate.ItemList.WeaponCrestDataList = item.WeaponCrestDataList;
                itemUpdate.ItemList.ArmorCrestDataList = item.ArmorCrestDataList;
                itemUpdate.ItemList.EquipElementParamList = item.EquipElementParamList;
                itemUpdate.UpdateItemNum = -((int)consumeItem.Num);
                ntc.UpdateItemList.Add(itemUpdate);

                // TODO: Figure out by ItemId
                uint goldValue = 100;
                uint amountToAdd = goldValue * consumeItem.Num;

                CDataWalletPoint characterWalletPoint = client.Character.WalletPointList.Where(wp => wp.Type == WalletType.Gold).First();
                characterWalletPoint.Value += amountToAdd; // TODO: Cap to maximum for that wallet
                Database.UpdateWalletPoint(client.Character.Id, characterWalletPoint);

                CDataUpdateWalletPoint walletUpdate = new CDataUpdateWalletPoint();
                walletUpdate.Type = WalletType.Gold;
                walletUpdate.AddPoint = (int) amountToAdd;
                walletUpdate.Value = characterWalletPoint.Value;
                ntc.UpdateWalletList.Add(walletUpdate);

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