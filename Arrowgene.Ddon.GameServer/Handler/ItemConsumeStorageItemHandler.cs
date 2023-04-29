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
    public class ItemConsumeStorageItemHandler : GameStructurePacketHandler<C2SItemConsumeStorageItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemConsumeStorageItemHandler));
        
        public ItemConsumeStorageItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SItemConsumeStorageItemReq> req)
        {
            client.Send(new S2CItemConsumeStorageItemRes());

            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc();
            ntc.UpdateType = 4;
            foreach (CDataStorageItemUIDList consumeItem in req.Structure.ConsumeItemList)
            {
                var tuple = client.Character.Storage.getStorageItem(consumeItem.StorageType, consumeItem.SlotNo);
                Item item = tuple.Item1;
                uint itemNum = tuple.Item2;

                itemNum = Math.Max(0, itemNum - consumeItem.Num);

                CDataItemUpdateResult ntcData0 = new CDataItemUpdateResult();
                ntcData0.ItemList.ItemUId = item.UId;
                ntcData0.ItemList.ItemId = item.ItemId;
                ntcData0.ItemList.ItemNum = itemNum;
                ntcData0.ItemList.Unk3 = item.Unk3;
                ntcData0.ItemList.StorageType = consumeItem.StorageType;
                ntcData0.ItemList.SlotNo = consumeItem.SlotNo;
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
                ntc.UpdateItemList.Add(ntcData0);

                if(itemNum == 0)
                {
                    // Delete item when ItemNum reaches 0 to free up the slot
                    client.Character.Storage.setStorageItem(null, 0, consumeItem.StorageType, consumeItem.SlotNo);
                    Server.Database.DeleteStorageItem(client.Character.CharacterId, consumeItem.StorageType, consumeItem.SlotNo);
                }
                else
                {
                    client.Character.Storage.setStorageItem(item, itemNum, consumeItem.StorageType, consumeItem.SlotNo);
                    Server.Database.ReplaceStorageItem(client.Character.CharacterId, consumeItem.StorageType, consumeItem.SlotNo, item.UId, itemNum);
                }

                client.Send(ntc);
            }
        }
    }
}