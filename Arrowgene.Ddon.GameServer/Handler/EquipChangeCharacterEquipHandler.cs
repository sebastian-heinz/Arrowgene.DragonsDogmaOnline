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

            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            updateCharacterItemNtc.UpdateType = 0x24;

            foreach (CDataCharacterEquipInfo changeCharacterEquipInfo in packet.Structure.ChangeCharacterEquipList)
            {
                EquipType equipType = (EquipType) changeCharacterEquipInfo.EquipType;
                byte equipSlot = changeCharacterEquipInfo.EquipCategory;

                if(changeCharacterEquipInfo.EquipItemUId.Length == 0)
                {
                    // Unequip item; from equipment to bag
                    // Find in equipment the item to unequip
                    Item item = client.Character.Equipment.getEquipItem(client.Character.Job, equipType, equipSlot);

                    client.Character.Equipment.setEquipItem(null, client.Character.Job, equipType, equipSlot);
                    Server.Database.DeleteEquipItem(client.Character.Id, client.Character.Job, equipType, equipSlot, item.UId);
                    
                    ushort dstSlotNo = client.Character.Storage.addStorageItem(item, StorageType.ItemBagEquipment);
                    Server.Database.InsertStorageItem(client.Character.Id, StorageType.ItemBagEquipment, dstSlotNo, item.UId);

                    updateCharacterItemNtc.UpdateItemList.Add(new CDataItemUpdateResult() {
                        UpdateItemNum = 0,
                        ItemList = new CDataItemList() {
                            ItemUId = item.UId,
                            ItemId = item.ItemId,
                            ItemNum = 1,
                            Unk3 = item.Unk3,
                            StorageType = (byte) StorageType.ItemBagEquipment,
                            SlotNo = dstSlotNo,
                            Unk6 = item.Color,
                            Unk7 = item.PlusValue,
                            Bind = true,
                            Unk9 = 0,
                            Unk10 = 0,
                            Unk11 = 0,
                            WeaponCrestDataList = item.WeaponCrestDataList,
                            ArmorCrestDataList = item.ArmorCrestDataList,
                            EquipElementParamList = item.EquipElementParamList
                        }
                    });
                }
                else
                {
                    // Equip item; from bag to equipment
                    // Find in the bag the item to equip
                    var tuple = client.Character.Storage.getStorage(StorageType.ItemBagEquipment)
                        .Select((item, index) => new {item = item, slot = (ushort) (index+1)})
                        .Where(tuple => tuple.item?.UId == changeCharacterEquipInfo.EquipItemUId)
                        .Single();
                    Item item = tuple.item;
                    ushort srcSlotNo = tuple.slot;

                    client.Character.Equipment.setEquipItem(item, client.Character.Job, (EquipType) changeCharacterEquipInfo.EquipType, changeCharacterEquipInfo.EquipCategory);
                    // TODO: Handle equipping over an already equipped slot
                    Server.Database.InsertEquipItem(client.Character.Id, client.Character.Job, equipType, equipSlot, item.UId);

                    // Find slot from which the item will be taken
                    client.Character.Storage.setStorageItem(null, StorageType.ItemBagEquipment, tuple.slot);
                    Server.Database.DeleteStorageItem(client.Character.Id, StorageType.ItemBagEquipment, tuple.slot);

                    updateCharacterItemNtc.UpdateItemList.Add(new CDataItemUpdateResult() {
                        UpdateItemNum = 0, // TODO: ?
                        ItemList = new CDataItemList() {
                            ItemUId = item.UId,
                            ItemId = item.ItemId,
                            ItemNum = 1,
                            Unk3 = item.Unk3,
                            StorageType = (byte) StorageType.ItemBagEquipment,
                            SlotNo = srcSlotNo,
                            Unk6 = item.Color,
                            Unk7 = item.PlusValue,
                            Bind = false,
                            Unk9 = 0,
                            Unk10 = client.Character.Id,
                            Unk11 = 0,
                            WeaponCrestDataList = item.WeaponCrestDataList,
                            ArmorCrestDataList = item.ArmorCrestDataList,
                            EquipElementParamList = item.EquipElementParamList
                        }
                    });
                }
            }

            S2CEquipChangeCharacterEquipRes res = new S2CEquipChangeCharacterEquipRes();
            res.CharacterEquipList = packet.Structure.ChangeCharacterEquipList;
            client.Send(res);

            client.Send(updateCharacterItemNtc);

            // Notify other players
            S2CEquipChangeCharacterEquipNtc changeCharacterEquipNtc = new S2CEquipChangeCharacterEquipNtc()
            {
                CharacterId = client.Character.Id,
                EquipItemList = client.Character.Equipment.getEquipmentAsCDataEquipItemInfo(client.Character.Job, EquipType.Performance),
                VisualEquipItemList = client.Character.Equipment.getEquipmentAsCDataEquipItemInfo(client.Character.Job, EquipType.Visual)
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
            
        }
    }
}