using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arrowgene.Ddon.Shared.Crypto;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class Item
    {
        private static string UIdPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public const int UIdLength = 8;

        public string UId { get; set; }
        public uint ItemId { get; set; }
        public byte Unk3 { get; set; } // QualityParam?
        public byte Color { get; set; }
        public byte PlusValue { get; set; }
        public List<CDataWeaponCrestData> WeaponCrestDataList { get; set; }
        public List<CDataArmorCrestData> ArmorCrestDataList { get; set; }
        public List<CDataEquipElementParam> EquipElementParamList { get; set; }

        public Item()
        {
            UId = GenerateEquipItemUId();
            WeaponCrestDataList = new List<CDataWeaponCrestData>();
            ArmorCrestDataList = new List<CDataArmorCrestData>();
            EquipElementParamList = new List<CDataEquipElementParam>();
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