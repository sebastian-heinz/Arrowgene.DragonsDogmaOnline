#nullable enable
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemUseJobItemsHandler : GameStructurePacketHandler<C2SItemUseJobItemsReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemUseJobItemsHandler));
        
        public ItemUseJobItemsHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SItemUseJobItemsReq> packet)
        {
            S2CItemUpdateCharacterItemNtc ntc = new();
            ntc.UpdateType = 0x121;

            foreach (CDataItemUIdList itemUIdListElement in packet.Structure.ItemUIdList)
            {
                (ushort slotNo, Item item, uint itemNum) = client.Character.Storage.getStorage(StorageType.ItemBagJob).findItemByUId(itemUIdListElement.UId)!;
                itemNum-=itemUIdListElement.Num;

                CDataItemUpdateResult itemUpdateResult = new();
                itemUpdateResult.ItemList.ItemUId = item.UId;
                itemUpdateResult.ItemList.ItemId = item.ItemId;
                itemUpdateResult.ItemList.ItemNum = itemNum;
                itemUpdateResult.ItemList.Unk3 = item.Unk3;
                itemUpdateResult.ItemList.StorageType = StorageType.ItemBagJob;
                itemUpdateResult.ItemList.SlotNo = slotNo;
                itemUpdateResult.ItemList.Color = item.Color; // ?
                itemUpdateResult.ItemList.PlusValue = item.PlusValue; // ?
                itemUpdateResult.ItemList.Bind = false;
                itemUpdateResult.ItemList.EquipPoint = 0;
                itemUpdateResult.ItemList.EquipCharacterID = 0;
                itemUpdateResult.ItemList.EquipPawnID = 0;
                itemUpdateResult.ItemList.WeaponCrestDataList = item.WeaponCrestDataList;
                itemUpdateResult.ItemList.ArmorCrestDataList = item.ArmorCrestDataList;
                itemUpdateResult.ItemList.EquipElementParamList = item.EquipElementParamList;
                itemUpdateResult.UpdateItemNum = (int)-itemUIdListElement.Num;
                ntc.UpdateItemList.Add(itemUpdateResult);

                if(itemNum == 0)
                {
                    // Delete item when ItemNum reaches 0 to free up the slot
                    client.Character.Storage.setStorageItem(null, 0, StorageType.ItemBagJob, slotNo);
                    Server.Database.DeleteStorageItem(client.Character.CharacterId, StorageType.ItemBagJob, slotNo);
                }
                else
                {
                    client.Character.Storage.setStorageItem(item, itemNum, StorageType.ItemBagJob, slotNo);
                    Server.Database.ReplaceStorageItem(client.Character.CharacterId, StorageType.ItemBagJob, slotNo, item.UId, itemNum);
                }
            }

            client.Send(ntc);
            client.Send(new S2CItemUseJobItemsRes());
        }
    }
}