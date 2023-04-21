using System;
using System.Collections.Generic;
using System.Linq;
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
                    ItemUId = tuple.item.Item1.UId,
                    ItemId = tuple.item.Item1.ItemId,
                    ItemNum = tuple.item.Item2,
                    Unk3 = tuple.item.Item1.Unk3,
                    StorageType = storageType,
                    SlotNo = tuple.slot,
                    Color = tuple.item.Item1.Color,
                    PlusValue = tuple.item.Item1.PlusValue,
                    Bind = true,
                    EquipPoint = 0,
                    EquipCharacterID = 0,
                    EquipPawnID = 0,
                    WeaponCrestDataList = tuple.item.Item1.WeaponCrestDataList,
                    ArmorCrestDataList = tuple.item.Item1.ArmorCrestDataList,
                    EquipElementParamList = tuple.item.Item1.EquipElementParamList
                })
                .ToList();
        }

        public Tuple<Item, uint>? getStorageItem(StorageType storageType, ushort slot) {
            return storages[storageType].Items[slot-1];
        }

        public ushort addStorageItem(Item newItem, uint itemCount, StorageType storageType) {
            // TODO: Limit itemCount to the item ID's max stack size in storageType
            var tuple = getStorage(storageType).Items
                .Select((item, index) => new {item = item, slot = (ushort) (index+1)})
                .Where(tuple => tuple.item == null)
                .First();
            setStorageItem(newItem, itemCount, storageType, tuple.slot);
            return tuple.slot;
        }

        public Tuple<Item, uint>? setStorageItem(Item newItem, uint itemCount, StorageType storageType, ushort slot) {
            // TODO: Limit itemCount to the item ID's max stack size in storageType
            Tuple<Item, uint>? oldItem = getStorageItem(storageType, slot);
            storages[storageType].Items[slot-1] = newItem == null
                ? null
                : new Tuple<Item, uint>(newItem, itemCount);
            return oldItem;
        }
    }

    public class Storage
    {
        public List<Tuple<Item, uint>?> Items { get; set; }
        public byte[] SortData { get; set; }

        public Storage(ushort slotMax) : this(slotMax, new byte[1024])
        {

        }

        public Storage(ushort slotMax, byte[] sortData)
        {
            Items = Enumerable.Repeat<Tuple<Item, uint>?>(null, slotMax).ToList();
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