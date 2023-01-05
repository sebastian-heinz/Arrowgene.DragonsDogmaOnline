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
    public class EquipChangeCharacterStorageEquipHandler : GameStructurePacketHandler<C2SEquipChangeCharacterStorageEquipReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EquipChangeCharacterStorageEquipHandler));

        public EquipChangeCharacterStorageEquipHandler(DdonGameServer server) : base(server)
        {
        }

        // Copypasted from EquipChangeCharacterEquipHandler
        // TODO: Move to an abstract class
        public override void Handle(GameClient client, StructurePacket<C2SEquipChangeCharacterStorageEquipReq> packet)
        {
            S2CItemUpdateCharacterItemNtc equipUpdateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            equipUpdateCharacterItemNtc.UpdateType = 0x26;

            S2CItemUpdateCharacterItemNtc unequipUpdateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc();
            unequipUpdateCharacterItemNtc.UpdateType = 0x26;

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
                    
                    ushort dstSlotNo = client.Character.Storage.addStorageItem(item, 1, StorageType.StorageBoxNormal);
                    Server.Database.InsertStorageItem(client.Character.Id, StorageType.StorageBoxNormal, dstSlotNo, item.UId, 1);

                    unequipUpdateCharacterItemNtc.UpdateItemList.Add(new CDataItemUpdateResult() {
                        UpdateItemNum = 0,
                        ItemList = new CDataItemList() {
                            ItemUId = item.UId,
                            ItemId = item.ItemId,
                            ItemNum = 1,
                            Unk3 = item.Unk3,
                            StorageType = (byte) StorageType.StorageBoxNormal,
                            SlotNo = dstSlotNo,
                            Color = item.Color,
                            PlusValue = item.PlusValue,
                            Bind = true,
                            EquipPoint = 0,
                            EquipCharacterID = 0,
                            EquipPawnID = 0,
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
                    var tuple = client.Character.Storage.getStorage(StorageType.StorageBoxNormal).Items
                        .Select((item, index) => new {item = item, slot = (ushort) (index+1)})
                        .Where(tuple => tuple.item?.Item1.UId == changeCharacterEquipInfo.EquipItemUId)
                        .Single();
                    Item item = tuple.item.Item1;
                    uint itemNum = tuple.item.Item2;
                    ushort srcSlotNo = tuple.slot;

                    client.Character.Equipment.setEquipItem(item, client.Character.Job, (EquipType) changeCharacterEquipInfo.EquipType, changeCharacterEquipInfo.EquipCategory);
                    // TODO: Handle equipping over an already equipped slot
                    Server.Database.InsertEquipItem(client.Character.Id, client.Character.Job, equipType, equipSlot, item.UId);

                    // Find slot from which the item will be taken
                    client.Character.Storage.setStorageItem(null, 0, StorageType.StorageBoxNormal, tuple.slot);
                    Server.Database.DeleteStorageItem(client.Character.Id, StorageType.StorageBoxNormal, tuple.slot);

                    equipUpdateCharacterItemNtc.UpdateItemList.Add(new CDataItemUpdateResult() {
                        UpdateItemNum = 0, // TODO: ?
                        ItemList = new CDataItemList() {
                            ItemUId = item.UId,
                            ItemId = item.ItemId,
                            ItemNum = 0,
                            Unk3 = item.Unk3,
                            StorageType = (byte) StorageType.StorageBoxNormal,
                            SlotNo = srcSlotNo,
                            Color = item.Color,
                            PlusValue = item.PlusValue,
                            Bind = true,
                            EquipPoint = 0,
                            EquipCharacterID = client.Character.Id,
                            EquipPawnID = 0,
                            WeaponCrestDataList = item.WeaponCrestDataList,
                            ArmorCrestDataList = item.ArmorCrestDataList,
                            EquipElementParamList = item.EquipElementParamList
                        }
                    });
                }
            }

            S2CEquipChangeCharacterStorageEquipRes res = new S2CEquipChangeCharacterStorageEquipRes();
            res.CharacterEquipList = packet.Structure.ChangeCharacterEquipList;
            client.Send(res);

            // Notify other players
            S2CEquipChangeCharacterEquipNtc changeCharacterEquipNtc = new S2CEquipChangeCharacterEquipNtc()
            {
                CharacterId = client.Character.Id,
                EquipItemList = client.Character.Equipment.getEquipmentAsCDataEquipItemInfo(client.Character.Job, EquipType.Performance),
                VisualEquipItemList = client.Character.Equipment.getEquipmentAsCDataEquipItemInfo(client.Character.Job, EquipType.Visual)
                // TODO: Unk0
            };

            foreach (Client otherClient in Server.ClientLookup.GetAll())
            {
                otherClient.Send(changeCharacterEquipNtc);
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