using Arrowgene.Ddon.GameServer.Chat.GatheringItem;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InstanceGetGatheringItemHandler : StructurePacketHandler<GameClient, C2SInstanceGetGatheringItemReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InstanceGetGatheringItemHandler));

        private static readonly StorageType DestinationStorageType = StorageType.ItemBagConsumable;

        private readonly GatheringItemManager _gatheringItemManager;

        public InstanceGetGatheringItemHandler(DdonGameServer server) : base(server)
        {
            this._gatheringItemManager = server.GatheringItemManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SInstanceGetGatheringItemReq> req)
        {
            S2CInstanceGetGatheringItemRes res = new S2CInstanceGetGatheringItemRes();
            res.LayoutId = req.Structure.LayoutId;
            res.PosId = req.Structure.PosId;
            res.GatheringItemGetRequestList = req.Structure.GatheringItemGetRequestList;
            client.Send(res);

            S2CItemUpdateCharacterItemNtc ntc = new S2CItemUpdateCharacterItemNtc();
            ntc.UpdateType = 1;
            foreach (CDataGatheringItemGetRequest gatheringItemRequest in req.Structure.GatheringItemGetRequestList)
            {
                CDataGatheringItemListUnk2 gatheredItem = this._gatheringItemManager.GetItems(req.Structure.LayoutId, req.Structure.PosId)
                    .Where(item => item.SlotNo == gatheringItemRequest.SlotNo)
                    .First();

                var tuple = client.Character.Storage.getStorage(DestinationStorageType).Items
                    .Select((item, index) => new {item = item, slot = (ushort) (index+1)})
                    .Where(tuple => tuple.item?.Item1.ItemId == gatheredItem.ItemId)
                    .FirstOrDefault();
                Item item = tuple?.item.Item1;
                uint itemNum = tuple?.item.Item2 ?? gatheringItemRequest.Num; // TODO: Cap to item bag stack maximum
                ushort slot = tuple?.slot ?? 0;

                if (item == null) {
                    item = new Item() {
                        ItemId = gatheredItem.ItemId,
                        Unk3 = 0,
                        Color = 0,
                        PlusValue = 0,
                        WeaponCrestDataList = new List<CDataWeaponCrestData>(),
                        ArmorCrestDataList = new List<CDataArmorCrestData>(),
                        EquipElementParamList = new List<CDataEquipElementParam>()
                    };
                    Server.Database.InsertItem(item);
                    slot = client.Character.Storage.addStorageItem(item, itemNum, DestinationStorageType);
                } else {
                    itemNum+=gatheringItemRequest.Num;
                }

                Server.Database.ReplaceStorageItem(client.Character.Id, DestinationStorageType, slot, item.UId, itemNum);

                CDataItemUpdateResult ntcData0 = new CDataItemUpdateResult();
                ntcData0.ItemList.ItemUId = item.UId;
                ntcData0.ItemList.ItemId = item.ItemId;
                ntcData0.ItemList.ItemNum = itemNum;
                ntcData0.ItemList.Unk3 = item.Unk3;
                ntcData0.ItemList.StorageType = (byte) DestinationStorageType;
                ntcData0.ItemList.SlotNo = slot;
                ntcData0.ItemList.Color = item.Color; // ?
                ntcData0.ItemList.PlusValue = item.PlusValue; // ?
                ntcData0.ItemList.Bind = false;
                ntcData0.ItemList.EquipPoint = 0;
                ntcData0.ItemList.EquipCharacterID = 0;
                ntcData0.ItemList.EquipPawnID = 0;
                ntcData0.ItemList.WeaponCrestDataList = item.WeaponCrestDataList;
                ntcData0.ItemList.ArmorCrestDataList = item.ArmorCrestDataList;
                ntcData0.ItemList.EquipElementParamList = item.EquipElementParamList;
                ntcData0.UpdateItemNum = (int)gatheringItemRequest.Num;
                ntc.UpdateItemList.Add(ntcData0);

                // Wallet points?
            }

            client.Send(ntc);
        }
    }
}
