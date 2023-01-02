using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    #nullable enable
    public class Storage
    {
        private Dictionary<StorageType, List<Item?>> storage;

        public Storage(Dictionary<StorageType, ushort> maxSlotsDict)
        {
            storage = new Dictionary<StorageType, List<Item?>>();
            foreach (var tuple in maxSlotsDict)
            {
                addStorage(tuple.Key, tuple.Value);
            }
        }

        public Dictionary<StorageType, List<Item?>> getAllStorages()
        {
            return storage;
        }

        public List<CDataCharacterItemSlotInfo> getAllStoragesAsCDataCharacterItemSlotInfoList()
        {
            return storage
                .Select(storage => new CDataCharacterItemSlotInfo() {
                    StorageType = storage.Key,
                    SlotMax = (ushort) storage.Value.Count
                })
                .ToList();
        }

        public List<Item?> getStorage(StorageType storageType)
        {
            return storage[storageType];
        }

        public void addStorage(StorageType storageType, ushort slotMax)
        {
            storage[storageType] = Enumerable.Repeat<Item?>(null, slotMax).ToList();
        }

        public List<CDataItemList> getStorageAsCDataItemList(StorageType storageType)
        {
            return getStorage(storageType)
                .Select((item, index) => new {item = item, slot = (ushort) (index+1)})
                .Where(tuple => tuple.item != null)
                .Select(tuple => new CDataItemList()
                {
                    ItemUId = tuple.item.UId,
                    ItemId = tuple.item.ItemId,
                    ItemNum = tuple.item.ItemNum,
                    Unk3 = tuple.item.Unk3,
                    StorageType = (byte) storageType,
                    SlotNo = tuple.slot,
                    Unk6 = tuple.item.Color,
                    Unk7 = tuple.item.PlusValue,
                    Bind = true,
                    Unk9 = 0,
                    Unk10 = 0,
                    Unk11 = 0,
                    WeaponCrestDataList = tuple.item.WeaponCrestDataList,
                    ArmorCrestDataList = tuple.item.ArmorCrestDataList,
                    EquipElementParamList = tuple.item.EquipElementParamList
                })
                .ToList();
        }

        public Item? getStorageItem(StorageType storageType, ushort slot) {
            return storage[storageType][slot-1];
        }

        public ushort addStorageItem(Item newItem, StorageType storageType) {
            var tuple = getStorage(storageType)
                .Select((item, index) => new {item = item, slot = (ushort) (index+1)})
                .Where(tuple => tuple.item == null)
                .First();
            setStorageItem(newItem, storageType, tuple.slot);
            return tuple.slot;
        }

        public Item? setStorageItem(Item newItem, StorageType storageType, ushort slot) {
            Item? oldItem = getStorageItem(storageType, slot);
            storage[storageType][slot-1] = newItem;
            return oldItem;
        }
    }

    // Check nItem::E_STORAGE_TYPE in the PS4 debug symbols for IDs?
    public enum StorageType : byte
    {
        ItemBagConsumable = 0x1,
        ItemBagMaterial = 0x2,
        ItemBagEquipment = 0x3, 
        ItemBagJob = 0x4,
        Unk5 = 0x5,
        StorageBoxNormal = 0x6,
        Unk7 = 0x7,
        Unk8 = 0x8,
        Unk9 = 0x9,
        Unk10 = 0xA,
        Unk11 = 0xB,
        Unk12 = 0xC,
        Unk13 = 0xD,
        Unk14 = 0xE,
        Unk15 = 0xF,
        Unk16 = 0x10,
        Unk17 = 0x11,
    }
}