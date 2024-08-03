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
        public uint EquipPoints { get; set; }
        public List<CDataEquipElementParam> EquipElementParamList { get; set; }
        public List<CDataAddStatusParam> AddStatusParamList { get; set; }
        public List<CDataEquipItemInfoUnk2> Unk2List { get; set; }

        private string _uid;

        public Item()
        {
            EquipElementParamList = new List<CDataEquipElementParam>();
            AddStatusParamList = new List<CDataAddStatusParam>();
            Unk2List = new List<CDataEquipItemInfoUnk2>();
        }

        public Item(Item obj)
        {
            this._uid = UpdateUId();
            this.ItemId = obj.ItemId;
            this.Unk3 = obj.Unk3;
            this.Color = obj.Color;
            this.PlusValue = obj.PlusValue;
            this.EquipPoints = obj.EquipPoints;
            // TODO: Make a copy constructor for these
            this.EquipElementParamList = obj.EquipElementParamList;
            this.AddStatusParamList = obj.AddStatusParamList;
            this.Unk2List = obj.Unk2List;
        }

        public string UpdateUId()
        {
            Random rnd = new Random();
            this._uid = $"{rnd.Next():X08}";
            return this._uid;
        }
    }
}
