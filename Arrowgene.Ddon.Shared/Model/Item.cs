using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class Item
    {
        public string UId
        {
            get
            {
                if (_uid == null)
                {
                    UpdateUId();
                }

                return _uid;
            }
            set => _uid = value;
        }

        public uint ItemId { get; set; }
        public byte SafetySetting { get; set; } // This is safety setting.
        public byte Color { get; set; }
        public byte PlusValue { get; set; } // This is Equipment Quality, +0/1/2/3/
        public uint EquipPoints { get; set; }
        public List<CDataEquipElementParam> EquipElementParamList { get; set; }
        public List<CDataAddStatusParam> AddStatusParamList { get; set; } // Actually LimitBreak/Bonus from Craig I guess.
        public List<CDataEquipItemInfoUnk2> Unk2List { get; set; } // Am thinking this might be addstatus but struggling to get this to work ingame.

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
            this.SafetySetting = obj.SafetySetting;
            this.Color = obj.Color;
            this.PlusValue = obj.PlusValue;
            this.EquipPoints = obj.EquipPoints;
            // TODO: Make a copy constructor for these
            this.EquipElementParamList = obj.EquipElementParamList;
            this.AddStatusParamList = obj.AddStatusParamList;
            this.Unk2List = obj.Unk2List;
        }

        private string UpdateUId()
        {
            _uid = Guid.CreateVersion7(DateTimeOffset.UtcNow).ToString();
            return _uid;
        }
    }
}
