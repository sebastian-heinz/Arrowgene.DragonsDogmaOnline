using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.Shared.Model
{
    public class ClientItemInfoAsset : Dictionary<ItemId, ClientItemInfo>
    {
        public ClientItemInfo this[uint itemId]
        {
            get => this[(ItemId)itemId];
            set => this[(ItemId)itemId] = value;
        }

        public void Add(uint itemId, ClientItemInfo item)
        {
            this[(ItemId)itemId] = item;
        }

        public bool ContainsKey(uint itemId)
        {
            return ContainsKey((ItemId)itemId);
        }
    }

    public class ClientItemInfo
    {
        public ItemId ItemId;
        public byte Category;
        public ushort Price;
        public byte StackLimit;
        public byte Rank;
        public string Name;
        public ItemSubCategory SubCategory;

        public byte? Level;
        public EquipJobList? JobGroup;
        public byte? CrestSlots;
        public byte? Quality;
        public Gender? Gender;

        public StorageType StorageType
        { 
            get
            {
                // For consumables, materials and equipment, itemlist.ipa Category
                // matches with StorageType values. But key and job items differ.
                if (Category == 4)
                    return StorageType.KeyItems;
                else if (Category == 5)
                    return StorageType.ItemBagJob;
                else
                    return (StorageType) Category;
            }
        }

        public HashSet<JobId>? JobIds
        {
            get
            {
                if (JobGroup == null) return null;
                return JobGroup switch
                {
                    EquipJobList.All => [.. Enum.GetValues<JobId>()],
                    EquipJobList.GroupHeavy => [JobId.Fighter, JobId.Warrior],
                    EquipJobList.GroupLight => [JobId.Seeker, JobId.Hunter, JobId.SpiritLancer],
                    EquipJobList.GroupPhysical => [
                        JobId.Fighter, JobId.Warrior,
                        JobId.Seeker, JobId.Hunter, JobId.SpiritLancer
                    ],
                    EquipJobList.GroupMagickRanged => [JobId.Priest, JobId.Sorcerer, JobId.ElementArcher],
                    EquipJobList.GroupMagickMelee => [JobId.ShieldSage, JobId.Alchemist, JobId.HighScepter],
                    EquipJobList.GroupMagickal => [
                        JobId.Priest, JobId.Sorcerer, JobId.ElementArcher,
                        JobId.ShieldSage, JobId.Alchemist, JobId.HighScepter
                    ],
                    _ => [(JobId)(int)JobGroup],
                };
            }
        }

        public EquipSlot? EquipSlot { get
            {
                if (SubCategory is ItemSubCategory.None) return null;
                else if (SubCategory > ItemSubCategory.JewelrySubCategoryOffset) return Model.EquipSlot.Jewelry1; //All Jewelry
                else if (SubCategory == ItemSubCategory.EquipEnsemble) return (Model.EquipSlot.ArmorBody); //Ensembles
                else if (SubCategory == ItemSubCategory.EquipLantern) return (Model.EquipSlot.Lantern); //Lanterns
                else if (SubCategory > ItemSubCategory.EquipSlotOffset) return (EquipSlot)(SubCategory - ItemSubCategory.EquipSlotOffset); //Other armor
                else if (SubCategory == ItemSubCategory.WeaponShield || SubCategory == ItemSubCategory.WeaponRod) return Model.EquipSlot.WepSub; //Subweapons
                else if (SubCategory > ItemSubCategory.WeaponCategoryOffset) return Model.EquipSlot.WepMain; //Main weapons
                else return null;
            } 
        }

        public RecipeCategory? RecipeCategory { get
            {
                switch (Category)
                {
                    case 1: //Use
                        return Model.RecipeCategory.Use;
                    case 2: //Material
                        return Model.RecipeCategory.Material;
                    case 3: //Equips
                        if (SubCategory > ItemSubCategory.JewelrySubCategoryOffset) return Model.RecipeCategory.EquipJewelry;

                        bool parsed = Enum.TryParse(SubCategory.ToString(), out RecipeCategory category);
                        if (parsed) return category;
                        else return null;
                    case 5: //Job Items
                        return Model.RecipeCategory.Job;
                    case 6: //Furniture
                        return Model.RecipeCategory.Furniture;
                    default:
                        return null;
                }
            } 
        }

        public override string ToString()
        {
            return $"{Name} <{ItemId}>";
        }
    }
}
