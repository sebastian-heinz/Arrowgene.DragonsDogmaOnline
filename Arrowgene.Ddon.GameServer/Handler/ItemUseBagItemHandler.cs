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
using Arrowgene.Ddon.GameServer.Characters;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ItemUseBagItemHandler : StructurePacketHandler<GameClient, C2SItemUseBagItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ItemUseBagItemHandler));

        private static readonly StorageType DestinationStorageType = StorageType.ItemBagConsumable;
        private DdonGameServer _Server;

        public ItemUseBagItemHandler(DdonGameServer server) : base(server)
        {
            _Server = server;
        }

        public override void Handle(GameClient client, StructurePacket<C2SItemUseBagItemReq> req)
        {
            S2CItemUseBagItemRes res = new S2CItemUseBagItemRes();
            client.Send(res);

            // TODO: Send S2CItemUseBagItemNtc?

            var tuple = client.Character.Storage.GetStorage(DestinationStorageType).Items
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

            if (_Server.ItemManager.IsSecretAbilityItem(item.ItemId))
            {
                _Server.JobManager.UnlockSecretAbility(client, client.Character, (SecretAbility) _Server.ItemManager.GetAbilityId(item.ItemId));
            }

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
            ntcData0.ItemList.EquipPoint = item.EquipPoints;
            ntcData0.ItemList.EquipCharacterID = 0;
            ntcData0.ItemList.EquipPawnID = 0;
            ntcData0.ItemList.EquipElementParamList = item.EquipElementParamList;
            ntcData0.ItemList.AddStatusParamList = item.AddStatusParamList;
            ntcData0.ItemList.Unk2List = item.Unk2List;
            ntcData0.UpdateItemNum = - (int) req.Structure.Amount;
            ntc.UpdateItemList.Add(ntcData0);

            if(itemNum == 0)
            {
                // Delete item when ItemNum reaches 0 to free up the slot
                client.Character.Storage.GetStorage(DestinationStorageType).SetItem(null, 0, slotNo);
                Server.Database.DeleteStorageItem(client.Character.ContentCharacterId, DestinationStorageType, slotNo);
            }
            else
            {
                client.Character.Storage.GetStorage(DestinationStorageType).SetItem(item, itemNum, slotNo);
                Server.Database.ReplaceStorageItem(client.Character.ContentCharacterId, DestinationStorageType, slotNo, itemNum, item);
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
