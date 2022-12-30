using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arrowgene.Ddon.Shared.Crypto;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class EquipItem
    {
        private static string UIdPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public const int UIdLength = 8;

        public EquipItem()
        {
            EquipItemUId = GenerateEquipItemUId();
            ItemId = 0;
            Unk0 = 0;
            EquipType = 0;
            EquipSlot = 0;
            Color = 0;
            PlusValue = 0;
            WeaponCrestDataList = new List<CDataWeaponCrestData>();
            ArmorCrestDataList = new List<CDataArmorCrestData>();
            EquipElementParamList = new List<CDataEquipElementParam>();
        }

        public EquipItem(CDataEquipItemInfo equipItemInfo) {
            EquipItemUId = GenerateEquipItemUId();
            ItemId = equipItemInfo.ItemId;
            Unk0 = equipItemInfo.Unk0;
            EquipType = equipItemInfo.EquipType;
            EquipSlot = equipItemInfo.EquipSlot;
            Color = equipItemInfo.Color;
            PlusValue = equipItemInfo.PlusValue;
            WeaponCrestDataList = equipItemInfo.WeaponCrestDataList;
            ArmorCrestDataList = equipItemInfo.ArmorCrestDataList;
            EquipElementParamList = equipItemInfo.EquipElementParamList;
        }

        public string EquipItemUId { get; set; }
        public uint ItemId { get; set; }
        public byte Unk0 { get; set; } // Not stored in DB cause i dont know what its for
        public byte EquipType { get; set; } // 1 = Equipment, 2 = Visual
        public ushort EquipSlot { get; set; }
        public byte Color { get; set; }
        public byte PlusValue { get; set; }
        public List<CDataWeaponCrestData> WeaponCrestDataList { get; set; }
        public List<CDataArmorCrestData> ArmorCrestDataList { get; set; }
        public List<CDataEquipElementParam> EquipElementParamList { get; set; }

        public CDataEquipItemInfo AsCDataEquipItemInfo() {
            return new CDataEquipItemInfo()
            {
                ItemId = this.ItemId,
                Unk0 = this.Unk0,
                EquipType = this.EquipType,
                EquipSlot = this.EquipSlot,
                Color = this.Color,
                PlusValue = this.PlusValue,
                WeaponCrestDataList = this.WeaponCrestDataList,
                ArmorCrestDataList = this.ArmorCrestDataList,
                EquipElementParamList = this.EquipElementParamList
            };
        }

        public CDataCharacterEquipInfo AsCDataCharacterEquipInfo() {
            return new CDataCharacterEquipInfo()
            {
                EquipItemUId = this.EquipItemUId,
                EquipType = this.EquipType,
                EquipCategory = (byte) this.EquipSlot // TODO: Check if these two values are equivalent or if some conversion is needed
            };
        }

        public static string GenerateEquipItemUId()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < UIdLength; i++)
            {
                int uidPoolIndex = CryptoRandom.Instance.Next(0, UIdPool.Length - 1);
                sb.Append(UIdPool[uidPoolIndex]);
            }
            return sb.ToString();
        }
    }
}