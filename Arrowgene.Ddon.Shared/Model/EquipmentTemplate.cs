using System;
using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    #nullable enable
    public class EquipmentTemplate
    {
        public static readonly byte TOTAL_EQUIP_SLOTS = 15;
        private static readonly byte TOTAL_JOB_ITEM_SLOT = 2; // TODO: Verify

        private readonly Dictionary<JobId, Dictionary<EquipType, List<Item?>>> equipment;
        private readonly Dictionary<JobId, List<Item?>> jobItems;

        public EquipmentTemplate(Dictionary<JobId, Dictionary<EquipType, List<Item?>>> equipment, Dictionary<JobId, List<Item?>> jobItems)
        {
            this.equipment = equipment;
            this.jobItems = jobItems;
        }
        
        public EquipmentTemplate()
        {
            equipment = new Dictionary<JobId, Dictionary<EquipType, List<Item?>>>();
            foreach (JobId job in Enum.GetValues(typeof(JobId)))
            {
                equipment[job] = new Dictionary<EquipType, List<Item?>>();
                foreach (EquipType equipType in Enum.GetValues(typeof(EquipType)))
                {
                    equipment[job][equipType] = Enumerable.Repeat<Item?>(null, TOTAL_EQUIP_SLOTS).ToList();
                }
            }

            jobItems = new Dictionary<JobId, List<Item?>>();
            foreach (JobId job in Enum.GetValues(typeof(JobId)))
            {
                jobItems[job] = Enumerable.Repeat<Item?>(null, TOTAL_JOB_ITEM_SLOT).ToList();
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

        public Item? GetEquipItem(JobId job, EquipType equipType, EquipSlot slot)
        {
            return GetEquipItem(job, equipType, (byte) slot);
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
        public List<CDataEquipItemInfo> EquipmentAsCDataEquipItemInfo(JobId job, EquipType equipType)
        {
            return GetEquipment(job, equipType)
                .Select((x, index) => new {item = x, slot = (byte)(index+1)})
                .Select(tuple => new CDataEquipItemInfo()
                {
                    ItemId = tuple.item?.ItemId ?? 0,
                    SafetySetting = tuple.item?.SafetySetting ?? 0,
                    EquipType = equipType,
                    EquipSlot = tuple.slot,
                    Color = tuple.item?.Color ?? 0,
                    PlusValue = tuple.item?.PlusValue ?? 0,
                    EquipElementParamList = tuple.item?.EquipElementParamList ?? new List<CDataEquipElementParam>(),
                    AddStatusParamList = tuple.item?.AddStatusParamList ?? new List<CDataAddStatusParam>(),
                    EquipStatParamList = tuple.item?.EquipStatParamList ?? new List<CDataEquipStatParam>()
                })
                .ToList();
        }

        public List<CDataEquipJobItem> JobItemsAsCDataEquipJobItem(JobId job)
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
