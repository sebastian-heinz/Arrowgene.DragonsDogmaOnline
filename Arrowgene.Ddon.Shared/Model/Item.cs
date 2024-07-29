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
#if false
            IncrementalHash hash = IncrementalHash.CreateHash(HashAlgorithmName.MD5); // It's for comparison, who cares, it's fast.
            hash.AppendData(BitConverter.GetBytes(ItemId));
            hash.AppendData(BitConverter.GetBytes(Unk3));
            hash.AppendData(BitConverter.GetBytes(Color));
            hash.AppendData(BitConverter.GetBytes(PlusValue));
            foreach (var weaponCrestData in WeaponCrestDataList)
            {
                hash.AppendData(BitConverter.GetBytes(weaponCrestData.SlotNo));
                hash.AppendData(BitConverter.GetBytes(weaponCrestData.CrestId));
                hash.AppendData(BitConverter.GetBytes(weaponCrestData.Add));
            }
            foreach (var armorCrestData in ArmorCrestDataList)
            {
                hash.AppendData(BitConverter.GetBytes(armorCrestData.u0));
                hash.AppendData(BitConverter.GetBytes(armorCrestData.u1));
                hash.AppendData(BitConverter.GetBytes(armorCrestData.u2));
                hash.AppendData(BitConverter.GetBytes(armorCrestData.u3));
            }
            foreach (var equipElementParam in EquipElementParamList)
            {
                hash.AppendData(BitConverter.GetBytes(equipElementParam.SlotNo));
                hash.AppendData(BitConverter.GetBytes(equipElementParam.ItemId));
            }
            this._uid = BitConverter.ToString(hash.GetHashAndReset()).Replace("-", string.Empty).Substring(0, UIdLength);
#else
            Random rnd = new Random();
            this._uid = $"{rnd.Next():X08}";
#endif
            return this._uid;
        }
    }
}
