using System.Collections.Generic;
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
    public class EquipChangeCharacterEquipHandler : GameStructurePacketHandler<C2SEquipChangeCharacterEquipReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipChangeCharacterEquipHandler));
        
        public EquipChangeCharacterEquipHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SEquipChangeCharacterEquipReq> packet)
        {
            S2CItemUpdateCharacterItemNtc equipUpdateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            equipUpdateCharacterItemNtc.UpdateType = 0x24;

            S2CItemUpdateCharacterItemNtc unequipUpdateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            unequipUpdateCharacterItemNtc.UpdateType = 0x24;

            foreach (CDataCharacterEquipInfo changeCharacterEquipInfo in packet.Structure.ChangeCharacterEquipList)
            {
                if(changeCharacterEquipInfo.EquipItemUId.Length == 0)
                {
                    // Unequip item; from equipment to bag
                    // Find in equipment the item to unequip
                    EquipItem equipItem = client.Character.CharacterEquipItemListDictionary[client.Character.Job]
                        .Concat(client.Character.CharacterEquipViewItemListDictionary[client.Character.Job])
                        .Where(ei => ei.EquipSlot == changeCharacterEquipInfo.EquipCategory
                                && ei.EquipType == changeCharacterEquipInfo.EquipType)
                        .Single();

                    List<EquipItem> targetEquipItemList = equipItem.EquipType == 1
                        ? client.Character.CharacterEquipItemListDictionary[client.Character.Job]
                        : client.Character.CharacterEquipViewItemListDictionary[client.Character.Job];

                    targetEquipItemList.Remove(equipItem);
                    Server.Database.DeleteEquipItem(client.Character.Id, client.Character.Job, equipItem);
                    
                    // Find slot in which to place the item
                    ushort dstSlotNo;
                    for (dstSlotNo = 0; dstSlotNo < client.Character.Items[StorageType.ItemBagEquipment].Count; dstSlotNo++)
                    {
                        if(client.Character.Items[StorageType.ItemBagEquipment][dstSlotNo] == null)
                        {
                            break;
                        }
                    }
                    dstSlotNo++;
                    client.Character.Items[StorageType.ItemBagEquipment][dstSlotNo-1] = equipItem;

                    unequipUpdateCharacterItemNtc.UpdateItemList.Add(new CDataItemUpdateResult() {
                        UpdateItemNum = 1,
                        ItemList = new CDataItemList() {
                            ItemUId = equipItem.EquipItemUId,
                            ItemId = equipItem.ItemId,
                            ItemNum = 1,
                            Unk3 = equipItem.Unk0,
                            StorageType = (byte) StorageType.ItemBagEquipment,
                            SlotNo = dstSlotNo,
                            Unk6 = equipItem.Color,
                            Unk7 = equipItem.PlusValue,
                            Bind = true,
                            Unk9 = 0,
                            Unk10 = 0,
                            Unk11 = 0,
                            WeaponCrestDataList = equipItem.WeaponCrestDataList,
                            ArmorCrestDataList = equipItem.ArmorCrestDataList,
                            EquipElementParamList = equipItem.EquipElementParamList
                        }
                    });
                }
                else
                {
                    // Equip item; from bag to equipment
                    // Find in the bag the item to equip
                    var tuple = client.Character.Items[StorageType.ItemBagEquipment]
                        .Select((item, index) => new {item, index})
                        .Where(tuple => tuple.item?.EquipItemUId == changeCharacterEquipInfo.EquipItemUId)
                        .Single();
                    EquipItem equipItem = tuple.item;
                    ushort srcSlotNo = (ushort)(tuple.index+1);

                    equipItem.EquipType = changeCharacterEquipInfo.EquipType;
                    equipItem.EquipSlot = changeCharacterEquipInfo.EquipCategory;

                    List<EquipItem> targetEquipItemList = changeCharacterEquipInfo.EquipType == 1
                        ? client.Character.CharacterEquipItemListDictionary[client.Character.Job]
                        : client.Character.CharacterEquipViewItemListDictionary[client.Character.Job];

                    targetEquipItemList.Add(equipItem);
                    // TODO: Handle equipping over an already equipped slot
                    Server.Database.InsertEquipItem(client.Character.Id, client.Character.Job, equipItem);

                    // Find slot from which the item will be taken
                    client.Character.Items[StorageType.ItemBagEquipment][srcSlotNo-1] = null;

                    equipUpdateCharacterItemNtc.UpdateItemList.Add(new CDataItemUpdateResult() {
                        UpdateItemNum = -1, // TODO: ?
                        ItemList = new CDataItemList() {
                            ItemUId = equipItem.EquipItemUId,
                            ItemId = equipItem.ItemId,
                            ItemNum = 0,
                            Unk3 = equipItem.Unk0,
                            StorageType = (byte) StorageType.ItemBagEquipment,
                            SlotNo = srcSlotNo,
                            Unk6 = equipItem.Color,
                            Unk7 = equipItem.PlusValue,
                            Bind = false,
                            Unk9 = 0,
                            Unk10 = 0,
                            Unk11 = 0,
                            WeaponCrestDataList = equipItem.WeaponCrestDataList,
                            ArmorCrestDataList = equipItem.ArmorCrestDataList,
                            EquipElementParamList = equipItem.EquipElementParamList
                        }
                    });
                }
            }

            S2CEquipChangeCharacterEquipRes res = new S2CEquipChangeCharacterEquipRes();
            res.CharacterEquipList = packet.Structure.ChangeCharacterEquipList;
            client.Send(res);


            // Notify other players
            S2CEquipChangeCharacterEquipNtc changeCharacterEquipNtc = new S2CEquipChangeCharacterEquipNtc()
            {
                CharacterId = client.Character.Id,
                EquipItemList = client.Character.CharacterEquipItemListDictionary[client.Character.Job]
                    .Select(x => x.AsCDataEquipItemInfo()).ToList(),
                VisualEquipItemList = client.Character.CharacterEquipViewItemListDictionary[client.Character.Job]
                    .Select(x => x.AsCDataEquipItemInfo()).ToList()
                // TODO: Unk0
            };

            // TODO: Check if it's necessary
            // S2CEquipChangeCharacterEquipLobbyNtc changeCharacterEquipLobbyNtc = new S2CEquipChangeCharacterEquipLobbyNtc()
            // {
            //     CharacterId = client.Character.Id,
            //     Job = client.Character.Job,
            //     EquipItemList = client.Character.CharacterEquipItemListDictionary[client.Character.Job]
            //         .Select(x => x.AsCDataEquipItemInfo()).ToList(),
            //     VisualEquipItemList = client.Character.CharacterEquipViewItemListDictionary[client.Character.Job]
            //         .Select(x => x.AsCDataEquipItemInfo()).ToList()
            // };

            foreach (Client otherClient in Server.ClientLookup.GetAll())
            {
                otherClient.Send(changeCharacterEquipNtc);
                //otherClient.Send(changeCharacterEquipLobbyNtc);
            }

            // This is the only way i've found for it to behave properly.
            // If i try to send equipping data (aka, removing from the bag the equipped item)
            //  without sending the first NTC even if it's empty AND SLEEPING (IMPORTANT), the item doesn't show up as equipped.
            // And if I don't send the equipping data, the item is duplicated, it appears as equipped AND still in the item bag
            // I have NO IDEA why you have to do it this way
            client.Send(unequipUpdateCharacterItemNtc);
            System.Threading.Thread.Sleep(100);
            client.Send(equipUpdateCharacterItemNtc);
        }
    }
}