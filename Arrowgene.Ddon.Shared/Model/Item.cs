using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class Item
    {
        public const int UIdLength = 8;

        public string UId { 
            get
            {
                if(this._uid == null)
                {
                    UpdateUId();
                }
                return this._uid;
            }
            set
            {
                this._uid = value;
            }
        }
        
        public uint ItemId { get; set; }
        public byte Unk3 { get; set; } // QualityParam?
        public byte Color { get; set; }
        public byte PlusValue { get; set; }
        public List<CDataWeaponCrestData> WeaponCrestDataList { get; set; }
        public List<CDataArmorCrestData> ArmorCrestDataList { get; set; }
        public List<CDataEquipElementParam> EquipElementParamList { get; set; }

        private string _uid;

        public Item()
        {
            WeaponCrestDataList = new List<CDataWeaponCrestData>();
            ArmorCrestDataList = new List<CDataArmorCrestData>();
            EquipElementParamList = new List<CDataEquipElementParam>();
        }

        public Item(Item obj)
        {
            this._uid = UpdateUId();
            this.ItemId = obj.ItemId;
            this.Unk3 = obj.Unk3;
            this.Color = obj.Color;
            this.PlusValue = obj.PlusValue;
            // TODO: Make a copy constructor for these
            this.WeaponCrestDataList = obj.WeaponCrestDataList;
            this.ArmorCrestDataList = obj.ArmorCrestDataList;
            this.EquipElementParamList = obj.EquipElementParamList;
        }

        public string UpdateUId()
        {
            Random rnd = new Random();
            this._uid = $"{rnd.Next():X08}";
            return this._uid;
        }
    }
}
