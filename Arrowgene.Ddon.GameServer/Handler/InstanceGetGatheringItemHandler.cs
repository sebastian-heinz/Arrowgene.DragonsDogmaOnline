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
                CDataGatheringItemListUnk2 gatheringItem = this._gatheringItemManager.GetItems(req.Structure.LayoutId, req.Structure.PosId)
                    .Where(item => item.SlotNo == gatheringItemRequest.SlotNo)
                    .Single();

                EquipItem item = client.Character.Items[DestinationStorageType]
                    .Where(item => item?.EquipType == (byte) DestinationStorageType && item?.ItemId == gatheringItem.ItemId)
                    .SingleOrDefault();

                if (item == null) {
                    int firstEmptySlotNo;
                    for (firstEmptySlotNo = 0; firstEmptySlotNo < client.Character.Items[DestinationStorageType].Count; firstEmptySlotNo++)
                    {
                        if(client.Character.Items[DestinationStorageType][firstEmptySlotNo] == null)
                        {
                            break;
                        }
                    }

                    item = new EquipItem() {
                        EquipItemUId = EquipItem.GenerateEquipItemUId(),
                        ItemId = gatheringItem.ItemId,
                        Unk0 = 0,
                        EquipType = (byte) DestinationStorageType,
                        EquipSlot = (ushort) (firstEmptySlotNo+1),
                        Color = 0,
                        PlusValue = 0,
                        WeaponCrestDataList = new List<CDataWeaponCrestData>(),
                        ArmorCrestDataList = new List<CDataArmorCrestData>(),
                        EquipElementParamList = new List<CDataEquipElementParam>()
                    };
                    client.Character.Items[DestinationStorageType][firstEmptySlotNo] = item;
                }

                CDataItemUpdateResult ntcData0 = new CDataItemUpdateResult();
                ntcData0.ItemList.ItemUId = item.EquipItemUId;
                ntcData0.ItemList.ItemId = item.ItemId;
                ntcData0.ItemList.ItemNum = /* TODO: previousValue + */gatheringItemRequest.Num; // Take only the requested amount, not gatheringItem.Num which would be all available
                ntcData0.ItemList.Unk3 = item.Unk0;
                ntcData0.ItemList.StorageType = item.EquipType;
                ntcData0.ItemList.SlotNo = item.EquipSlot;
                ntcData0.ItemList.Unk6 = item.Color; // ?
                ntcData0.ItemList.Unk7 = item.PlusValue; // ?
                ntcData0.ItemList.Bind = false;
                ntcData0.ItemList.Unk9 = 0;
                ntcData0.ItemList.Unk10 = 0;
                ntcData0.ItemList.Unk11 = 0;
                ntcData0.ItemList.WeaponCrestDataList = item.WeaponCrestDataList;
                ntcData0.ItemList.ArmorCrestDataList = item.ArmorCrestDataList;
                ntcData0.ItemList.EquipElementParamList = item.EquipElementParamList;
                ntcData0.UpdateItemNum = 0;
                ntc.UpdateItemList.Add(ntcData0);

                // Wallet points?
            }

            client.Send(ntc);
        }
    }
}
