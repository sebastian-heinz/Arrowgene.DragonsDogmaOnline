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
        public List<CDataEquipElementParam> EquipElementParamList { get; set; }
        public List<CDataEquipItemInfoUnk1> Unk1 { get; set; }
        public List<CDataEquipItemInfoUnk2> Unk2 { get; set; }

        private string _uid;

        public Item()
        {
            EquipElementParamList = new List<CDataEquipElementParam>();
            Unk1 = new List<CDataEquipItemInfoUnk1>();
            Unk2 = new List<CDataEquipItemInfoUnk2>();
        }

        public string UpdateUId()
        {
            IncrementalHash hash = IncrementalHash.CreateHash(HashAlgorithmName.MD5); // It's for comparison, who cares, it's fast.
            hash.AppendData(BitConverter.GetBytes(ItemId));
            hash.AppendData(BitConverter.GetBytes(Unk3));
            hash.AppendData(BitConverter.GetBytes(Color));
            hash.AppendData(BitConverter.GetBytes(PlusValue));
            foreach (var weaponCrestData in EquipElementParamList)
            {
                hash.AppendData(BitConverter.GetBytes(weaponCrestData.SlotNo));
                hash.AppendData(BitConverter.GetBytes(weaponCrestData.CrestId));
                hash.AppendData(BitConverter.GetBytes(weaponCrestData.Add));
            }
            foreach (var armorCrestData in Unk1)
            {
                hash.AppendData(BitConverter.GetBytes(armorCrestData.u0));
                hash.AppendData(BitConverter.GetBytes(armorCrestData.u1));
                hash.AppendData(BitConverter.GetBytes(armorCrestData.u2));
                hash.AppendData(BitConverter.GetBytes(armorCrestData.u3));
            }
            foreach (var equipElementParam in Unk2)
            {
                hash.AppendData(BitConverter.GetBytes(equipElementParam.SlotNo));
                hash.AppendData(BitConverter.GetBytes(equipElementParam.ItemId));
            }
            this._uid = BitConverter.ToString(hash.GetHashAndReset()).Replace("-", string.Empty).Substring(0, UIdLength);
            return this._uid;
        }
    }
}
