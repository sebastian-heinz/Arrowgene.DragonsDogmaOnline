using System;
using Arrowgene.Ddon.GameServer.GatheringItems;
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

        private static readonly Dictionary<uint, (byte WalletType, uint Quantity)> ItemIdWalletTypeAndQuantity = new Dictionary<uint, (byte WalletType, uint Amount)>() { 
            {7789, (1, 1)},
            {7790, (1, 10)},
            {7791, (1, 100)},
            {7792, (2,1)},
            {7793, (2,10)},
            {7794, (2,100)},
            {7795, (3,1)}, // Doesn't show up 
            {7796, (3,10)}, // Doesn't show up
            {7797, (3,100)}, // Doesn't show up
            {18742, (9,1)}, // TODO: Check if HO is type 9
            {18743, (9,10)},
            {18744, (9,100)},
            {18828,(1,7500)},
            {18829,(2,1250)},
            {18830,(3,750)},
            {19508,(1,1000)},
            {19509,(1,10000)},
            {19510,(2,1000)},
            {19511,(3,1000)}
            // TODO: Find all items that add wallet points
        };

        // [[item]]
        // id = 16822 (Adds 100 XP)
        // old = '経験値結晶'
        // new = 'Experience Crystal'
        // [[item]]
        // id = 16831 (Adds 10000 XP)
        // old = '経験値結晶'
        // new = 'Experience Crystal'
        // [[item]]
        // id = 18831 (Adds 63000 XP)
        // old = '経験値結晶'
        // new = 'Experience Crystal'

        // [[item]]
        // id = 18832 (Adds 18 PP)
        // old = 'プレイポイント'
        // new = 'Play Point'
        // [[item]]
        // id = 25651 (Adds 1 PP)
        // old = 'プレイポイント'
        // new = 'Play Point'
        // [[item]]
        // id = 25652 (Adds 10 PP)
        // old = 'プレイポイント'
        // new = 'Play Point'
        // [[item]]
        // id = 25653 (Adds 100 PP)
        // old = 'プレイポイント'
        // new = 'Play Point'

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
                GatheringItem gatheredItem = client.InstanceGatheringItemManager.GetAssets(req.Structure.LayoutId, req.Structure.PosId)[(int) gatheringItemRequest.SlotNo];
                uint pickedGatherItems;

                if(ItemIdWalletTypeAndQuantity.ContainsKey(gatheredItem.ItemId)) {
                    var walletTypeAndQuantity = ItemIdWalletTypeAndQuantity[gatheredItem.ItemId];

                    pickedGatherItems = gatheringItemRequest.Num;
                    uint totalQuantityToAdd = walletTypeAndQuantity.Quantity * gatheredItem.ItemNum;
                    
                    CDataWalletPoint characterWalletPoint = client.Character.WalletPointList.Where(wp => wp.Type == walletTypeAndQuantity.WalletType).First();
                    characterWalletPoint.Value += totalQuantityToAdd; // TODO: Cap to maximum for that wallet
                    // TODO: Save to DB

                    CDataUpdateWalletPoint walletUpdate = new CDataUpdateWalletPoint();
                    walletUpdate.Type = walletTypeAndQuantity.WalletType;
                    walletUpdate.AddPoint = (int) totalQuantityToAdd;
                    walletUpdate.Value = characterWalletPoint.Value;
                    ntc.UpdateWalletList.Add(walletUpdate);
                } else {
                    // TODO: Determine by gatheredItem.ItemId
                    StorageType destinationStorageType = StorageType.ItemBagConsumable;

                    pickedGatherItems = gatheringItemRequest.Num;

                    var tuple = client.Character.Storage.getStorage(destinationStorageType).Items
                        .Select((item, index) => new {item = item, slot = (ushort) (index+1)})
                        .Where(tuple => tuple.item?.Item1.ItemId == gatheredItem.ItemId)
                        .FirstOrDefault();
                    Item item = tuple?.item.Item1;
                    uint oldItemNum = tuple?.item.Item2 ?? 0;
                    uint newItemNum = oldItemNum + pickedGatherItems; // TODO: Cap to item bag stack maximum
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
                        slot = client.Character.Storage.addStorageItem(item, newItemNum, destinationStorageType);
                    } else {
                        client.Character.Storage.setStorageItem(item, newItemNum, destinationStorageType, slot);
                    }

                    Server.Database.ReplaceStorageItem(client.Character.Id, destinationStorageType, slot, item.UId, newItemNum);

                    CDataItemUpdateResult ntcData0 = new CDataItemUpdateResult();
                    ntcData0.ItemList.ItemUId = item.UId;
                    ntcData0.ItemList.ItemId = item.ItemId;
                    ntcData0.ItemList.ItemNum = newItemNum;
                    ntcData0.ItemList.Unk3 = item.Unk3;
                    ntcData0.ItemList.StorageType = destinationStorageType;
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
                    ntcData0.UpdateItemNum = (int) gatheringItemRequest.Num;
                    ntc.UpdateItemList.Add(ntcData0);
                }

                gatheredItem.ItemNum -= pickedGatherItems;
                // TODO: Maybe remove gatheredItem when ItemNum reaches 0? Doesn't seem to be needed
            }

            client.Send(ntc);
        }
    }
}
