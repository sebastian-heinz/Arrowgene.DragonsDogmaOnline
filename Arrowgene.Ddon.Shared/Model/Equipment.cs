using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    #nullable enable
    public class Equipment
    {
        private static readonly byte EQUIP_SLOT_NUMBER = 15;
        private static readonly byte JOB_ITEM_SLOT_NUMBER = 2; // TODO: Verify

        private readonly Dictionary<JobId, Dictionary<EquipType, List<Item?>>> equipment;
        private readonly Dictionary<JobId, List<Item?>> jobItems;

        public Equipment(Dictionary<JobId, Dictionary<EquipType, List<Item?>>> equipment, Dictionary<JobId, List<Item?>> jobItems)
        {
            this.equipment = equipment;
            this.jobItems = jobItems;
        }
        
        public Equipment()
        {
            equipment = new Dictionary<JobId, Dictionary<EquipType, List<Item?>>>();
            foreach (JobId job in Enum.GetValues(typeof(JobId)))
            {
                equipment[job] = new Dictionary<EquipType, List<Item?>>();
                foreach (EquipType equipType in Enum.GetValues(typeof(EquipType)))
                {
                    equipment[job][equipType] = Enumerable.Repeat<Item?>(null, EQUIP_SLOT_NUMBER).ToList();
                }
            }

            jobItems = new Dictionary<JobId, List<Item?>>();
            foreach (JobId job in Enum.GetValues(typeof(JobId)))
            {
                jobItems[job] = Enumerable.Repeat<Item?>(null, JOB_ITEM_SLOT_NUMBER).ToList();
            }
        }


        public Dictionary<JobId, Dictionary<EquipType, List<Item?>>> GetAllEquipment()
        {
            return equipment;
        }

        public List<Item?> GetEquipment(JobId job, EquipType equipType)
        {
            return equipment[job][equipType];
        }

        public Item? GetEquipItem(JobId job, EquipType equipType, byte slot) {
            return equipment[job][equipType][slot-1];
        }

        public Item? SetEquipItem(Item? newItem, JobId job, EquipType equipType, byte slot) {
            Item? oldItem = GetEquipItem(job, equipType, slot);
            equipment[job][equipType][slot-1] = newItem;
            return oldItem;
        }


        public Dictionary<JobId, List<Item?>> GetAllJobItems()
        {
            return jobItems;
        }

        public List<Item?> GetJobItems(JobId job)
        {
            return jobItems[job];
        }

        public Item? GetJobItem(JobId job, byte slot) {
            return jobItems[job][slot-1];
        }

        public Item? SetJobItem(Item? newItem, JobId job, byte slot) {
            Item? oldItem = GetJobItem(job, slot);
            jobItems[job][slot-1] = newItem;
            return oldItem;
        }


        public List<CDataContextEquipData> getEquipmentAsCDataContextEquipData(JobId job, EquipType equipType)
        {
            // In the context equipment lists, the index is the slot. An element with all info set to 0 has to be in place if a slot is not filled
            return GetEquipment(job, equipType)
                .Select(x => x == null ? new CDataContextEquipData() : new CDataContextEquipData()
                {
                    ItemId = (ushort) x.ItemId,
                    ColorNo = x.Color,
                    QualityParam = x.Unk3,
                    WeaponCrestDataList = x.WeaponCrestDataList,
                    ArmorCrestDataList = x.ArmorCrestDataList
                })
                .ToList();
        }

        public List<CDataEquipItemInfo> getEquipmentAsCDataEquipItemInfo(JobId job, EquipType equipType)
        {
            return GetEquipment(job, equipType)
                .Select((x, index) => new {item = x, slot = (byte)(index+1)})
                .Select(tuple => new CDataEquipItemInfo()
                {
                    ItemId = tuple.item?.ItemId ?? 0,
                    Unk0 = tuple.item?.Unk3 ?? 0,
                    EquipType = equipType,
                    EquipSlot = tuple.slot,
                    Color = tuple.item?.Color ?? 0,
                    PlusValue = tuple.item?.PlusValue ?? 0,
                    WeaponCrestDataList = tuple.item?.WeaponCrestDataList ?? new List<CDataWeaponCrestData>(),
                    ArmorCrestDataList = tuple.item?.ArmorCrestDataList ?? new List<CDataArmorCrestData>(),
                    EquipElementParamList = tuple.item?.EquipElementParamList ?? new List<CDataEquipElementParam>()
                })
                .ToList();
        }

        public List<CDataCharacterEquipInfo> getEquipmentAsCDataCharacterEquipInfo(JobId job, EquipType equipType)
        {
            return GetEquipment(job, equipType)
                .Select((x, index) => new {item = x, slot = (byte)(index+1)})
                .Where(tuple => tuple.item != null)
                .Select(tuple => new CDataCharacterEquipInfo()
                {
                    EquipItemUId = tuple.item.UId,
                    EquipType = (byte) equipType,
                    EquipCategory = tuple.slot
                })
                .ToList();
        }


        public List<CDataEquipJobItem> getJobItemsAsCDataEquipJobItem(JobId job)
        {
            return GetJobItems(job)
                .Select((x, index) => new {itemAndNum = x, slot = (byte)(index+1)})
                .Select(tuple => new CDataEquipJobItem()
                {
                    JobItemId = tuple.itemAndNum?.ItemId ?? 0,
                    EquipSlotNo = tuple.slot
                })
                .ToList();
        }
    }

    public enum EquipType : byte {
        Performance = 1,
        Visual = 2
    }
}