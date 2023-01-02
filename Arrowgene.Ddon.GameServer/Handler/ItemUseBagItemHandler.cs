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

            var tuple = client.Character.Storage.getStorage(DestinationStorageType)
                .Select((x, index) => new {item = x, slot = index+1})
                .Where(tuple => tuple.item?.UId == req.Structure.ItemUId)
                .Single();
            Item item = tuple.item;
            ushort slotNo = (ushort) tuple.slot;

            item.ItemNum--;
            Server.Database.UpdateItem(item);

            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc();
            ntc.UpdateType = 3;

            CDataItemUpdateResult ntcData0 = new CDataItemUpdateResult();
            ntcData0.ItemList.ItemUId = item.UId;
            ntcData0.ItemList.ItemId = item.ItemId;
            ntcData0.ItemList.ItemNum = item.ItemNum;
            ntcData0.ItemList.Unk3 = item.Unk3;
            ntcData0.ItemList.StorageType = (byte) DestinationStorageType;
            ntcData0.ItemList.SlotNo = slotNo;
            ntcData0.ItemList.Unk6 = item.Color; // ?
            ntcData0.ItemList.Unk7 = item.PlusValue; // ?
            ntcData0.ItemList.Bind = false;
            ntcData0.ItemList.Unk9 = 0;
            ntcData0.ItemList.Unk10 = 0;
            ntcData0.ItemList.Unk11 = 0;
            ntcData0.ItemList.WeaponCrestDataList = item.WeaponCrestDataList;
            ntcData0.ItemList.ArmorCrestDataList = item.ArmorCrestDataList;
            ntcData0.ItemList.EquipElementParamList = item.EquipElementParamList;
            ntcData0.UpdateItemNum = -1;
            ntc.UpdateItemList.Add(ntcData0);

            // Wallet points?

            client.Send(ntc);

            // Lantern start NTC
            // TODO: Figure out all item IDs that do lantern stuff
            if (item.ItemId == 55)
            { 
                client.Send(SelectedDump.lantern2_27_16); 
                // TODO: Send S2C_CHARACTER_START_LANTERN_OTHER_NOTICE to other party members
            }
        }
    }
}
