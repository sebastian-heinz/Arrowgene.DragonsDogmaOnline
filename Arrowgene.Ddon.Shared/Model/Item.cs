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
        public byte Unk3 { get; set; } // This is safety setting.
        public byte Color { get; set; }
        public byte PlusValue { get; set; } // This is Equipment Quality, +0/1/2/3/
        public uint EquipPoints { get; set;}
        public List<CDataWeaponCrestData> WeaponCrestDataList { get; set; }
        public List<CDataAddStatusData> AddStatusData { get; set; }
        public List<CDataEquipElementParam> EquipElementParamList { get; set; }

        private string _uid;

        public Item()
        {
            WeaponCrestDataList = new List<CDataWeaponCrestData>();
            AddStatusData = new List<CDataAddStatusData>();
            EquipElementParamList = new List<CDataEquipElementParam>();
        }

        public string UpdateUId()
        {
            IncrementalHash hash = IncrementalHash.CreateHash(HashAlgorithmName.MD5); // It's for comparison, who cares, it's fast.
            hash.AppendData(BitConverter.GetBytes(ItemId));
            hash.AppendData(BitConverter.GetBytes(Unk3));
            hash.AppendData(BitConverter.GetBytes(Color));
            hash.AppendData(BitConverter.GetBytes(PlusValue));
            hash.AppendData(BitConverter.GetBytes(EquipPoints));
            foreach (var weaponCrestData in WeaponCrestDataList)
            {
                hash.AppendData(BitConverter.GetBytes(weaponCrestData.SlotNo));
                hash.AppendData(BitConverter.GetBytes(weaponCrestData.CrestId));
                hash.AppendData(BitConverter.GetBytes(weaponCrestData.Add));
            }
            foreach (var addStatData in AddStatusData)
            {
                hash.AppendData(BitConverter.GetBytes(addStatData.IsAddStat1));
                hash.AppendData(BitConverter.GetBytes(addStatData.IsAddStat2));
                hash.AppendData(BitConverter.GetBytes(addStatData.AdditionalStatus1));
                hash.AppendData(BitConverter.GetBytes(addStatData.AdditionalStatus2));
            }
            foreach (var equipElementParam in EquipElementParamList)
            {
                hash.AppendData(BitConverter.GetBytes(equipElementParam.SlotNo));
                hash.AppendData(BitConverter.GetBytes(equipElementParam.ItemId));
            }
            this._uid = BitConverter.ToString(hash.GetHashAndReset()).Replace("-", string.Empty).Substring(0, UIdLength);
            return this._uid;
        }
    }
}