using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemUseBagItemHandler : StructurePacketHandler<GameClient, C2SItemUseBagItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemUseBagItemHandler));

        private static readonly StorageType DestinationStorageType = StorageType.ItemBagConsumable;

        public ItemUseBagItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SItemUseBagItemReq> req)
        {
            S2CItemUseBagItemRes res = new S2CItemUseBagItemRes();
            client.Send(res);

            // TODO: Send S2CItemUseBagItemNtc?

            var tuple = client.Character.Storage.getStorage(DestinationStorageType).Items
                .Select((x, index) => new {item = x, slot = index+1})
                .Where(tuple => tuple.item?.Item1.UId == req.Structure.ItemUId)
                .First();
            Item item = tuple.item.Item1;
            uint itemNum = tuple.item.Item2;
            ushort slotNo = (ushort) tuple.slot;

            itemNum--;

            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.UseBag
            };

            CDataItemUpdateResult ntcData0 = new CDataItemUpdateResult();
            ntcData0.ItemList.ItemUId = item.UId;
            ntcData0.ItemList.ItemId = item.ItemId;
            ntcData0.ItemList.ItemNum = itemNum;
            ntcData0.ItemList.Unk3 = item.Unk3;
            ntcData0.ItemList.StorageType = DestinationStorageType;
            ntcData0.ItemList.SlotNo = slotNo;
            ntcData0.ItemList.Color = item.Color; // ?
            ntcData0.ItemList.PlusValue = item.PlusValue; // ?
            ntcData0.ItemList.Bind = false;
            ntcData0.ItemList.EquipPoint = 0;
            ntcData0.ItemList.EquipCharacterID = 0;
            ntcData0.ItemList.EquipPawnID = 0;
            ntcData0.ItemList.EquipElementParamList = item.EquipElementParamList;
            ntcData0.ItemList.Unk1 = item.Unk1;
            ntcData0.ItemList.Unk2 = item.Unk2;
            ntcData0.UpdateItemNum = - (int) req.Structure.Amount;
            ntc.UpdateItemList.Add(ntcData0);

            if(itemNum == 0)
            {
                // Delete item when ItemNum reaches 0 to free up the slot
                client.Character.Storage.setStorageItem(null, 0, DestinationStorageType, slotNo);
                Server.Database.DeleteStorageItem(client.Character.CharacterId, DestinationStorageType, slotNo);
            }
            else
            {
                client.Character.Storage.setStorageItem(item, itemNum, DestinationStorageType, slotNo);
                Server.Database.ReplaceStorageItem(client.Character.CharacterId, DestinationStorageType, slotNo, item.UId, itemNum);
            }

            client.Send(ntc);

            // Lantern start NTC
            // TODO: Figure out all item IDs that do lantern stuff
            if (item.ItemId == 55)
            { 
                client.Send(SelectedDump.lantern2_27_16); 
                // TODO: Send S2C_CHARACTER_START_LANTERN_OTHER_NOTICE to other party members?
            }
        }
    }
}
