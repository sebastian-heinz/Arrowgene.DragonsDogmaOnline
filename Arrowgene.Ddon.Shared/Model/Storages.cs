using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    #nullable enable
    public class Storages
    {
        private Dictionary<StorageType, Storage> storages;

        public Storages(Dictionary<StorageType, ushort> maxSlotsDict)
        {
            storages = new Dictionary<StorageType, Storage>();
            foreach (var tuple in maxSlotsDict)
            {
                addStorage(tuple.Key, new Storage(tuple.Value));
            }
        }

        public Dictionary<StorageType, Storage> getAllStorages()
        {
            return storages;
        }

        public List<CDataCharacterItemSlotInfo> getAllStoragesAsCDataCharacterItemSlotInfoList()
        {
            return storages
                .Select(storage => new CDataCharacterItemSlotInfo() {
                    StorageType = storage.Key,
                    SlotMax = (ushort) storage.Value.Items.Count
                })
                .ToList();
        }

        public Storage getStorage(StorageType storageType)
        {
            return storages[storageType];
        }

        public void addStorage(StorageType storageType, Storage storage)
        {
            storages[storageType] = storage;
        }

        public List<CDataItemList> getStorageAsCDataItemList(StorageType storageType)
        {
            return getStorage(storageType).Items
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
            return storages[storageType].Items[slot-1];
        }

        public ushort addStorageItem(Item newItem, StorageType storageType) {
            var tuple = getStorage(storageType).Items
                .Select((item, index) => new {item = item, slot = (ushort) (index+1)})
                .Where(tuple => tuple.item == null)
                .First();
            setStorageItem(newItem, storageType, tuple.slot);
            return tuple.slot;
        }

        public Item? setStorageItem(Item newItem, StorageType storageType, ushort slot) {
            Item? oldItem = getStorageItem(storageType, slot);
            storages[storageType].Items[slot-1] = newItem;
            return oldItem;
        }
    }

    public class Storage
    {
        public List<Item?> Items { get; set; }
        public byte[] SortData { get; set; }

        public Storage(ushort slotMax) : this(slotMax, new byte[1024])
        {

        }

        public Storage(ushort slotMax, byte[] sortData)
        {
            Items = Enumerable.Repeat<Item?>(null, slotMax).ToList();
            SortData = sortData;
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