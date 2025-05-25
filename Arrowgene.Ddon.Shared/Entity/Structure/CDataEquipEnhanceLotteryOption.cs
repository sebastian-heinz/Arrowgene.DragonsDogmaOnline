using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEquipEnhanceLotteryOption
    {
        public CDataEquipEnhanceLotteryOption() {
            RowTitle = string.Empty;
            WalletPointCost = new List<CDataWalletPoint>();
            ItemCost = new List<CDataItemAmount>();
            Unk7 = new List<CDataWalletPoint>();
            Unk8 = new List<CDataItemAmount>();
            ShopTypeListings = new List<CDataCommonU8>();
            MainSuccessExample = new List<CDataS2CEquipEnhancedGetPacksResUnk0Unk10>();
        }

        public ushort Index { get; set; } // Some kind of an ID, all items with same ID display same ROW?
        public byte Category { get; set; } // Determines the position in the list
        public string RowTitle { get; set; }
        public ushort RetryLimited { get; set; } // Prints "Retry Limited"  @ Craig when set to 1
        public ushort AttemptModifier { get; set; } // In Limit break reports "first time only" @ Craig when set to 1
        public List<CDataWalletPoint> WalletPointCost { get; set; } // Currency Required for option
        public List<CDataItemAmount> ItemCost { get; set; } // Trade In required
        public List<CDataWalletPoint> Unk7 { get; set; }
        public List<CDataItemAmount> Unk8 { get; set; }
        public List<CDataCommonU8> ShopTypeListings { get; set; } // 1 = Weapon, 5 = Armor
        public List<CDataS2CEquipEnhancedGetPacksResUnk0Unk10> MainSuccessExample { get; set; } // Main Success Example?
    
        public class Serializer : EntitySerializer<CDataEquipEnhanceLotteryOption>
        {
            public override void Write(IBuffer buffer, CDataEquipEnhanceLotteryOption obj)
            {
                WriteUInt16(buffer, obj.Index);
                WriteByte(buffer, obj.Category);
                WriteMtString(buffer, obj.RowTitle);
                WriteUInt16(buffer, obj.RetryLimited);
                WriteUInt16(buffer, obj.AttemptModifier);
                WriteEntityList<CDataWalletPoint>(buffer, obj.WalletPointCost);
                WriteEntityList<CDataItemAmount>(buffer, obj.ItemCost);
                WriteEntityList<CDataWalletPoint>(buffer, obj.Unk7);
                WriteEntityList<CDataItemAmount>(buffer, obj.Unk8);
                WriteEntityList<CDataCommonU8>(buffer, obj.ShopTypeListings);
                WriteEntityList<CDataS2CEquipEnhancedGetPacksResUnk0Unk10>(buffer, obj.MainSuccessExample);
            }
        
            public override CDataEquipEnhanceLotteryOption Read(IBuffer buffer)
            {
                CDataEquipEnhanceLotteryOption obj = new CDataEquipEnhanceLotteryOption();
                obj.Index = ReadUInt16(buffer);
                obj.Category = ReadByte(buffer);
                obj.RowTitle = ReadMtString(buffer);
                obj.RetryLimited = ReadUInt16(buffer);
                obj.AttemptModifier = ReadUInt16(buffer);
                obj.WalletPointCost = ReadEntityList<CDataWalletPoint>(buffer);
                obj.ItemCost = ReadEntityList<CDataItemAmount>(buffer);
                obj.Unk7 = ReadEntityList<CDataWalletPoint>(buffer);
                obj.Unk8 = ReadEntityList<CDataItemAmount>(buffer);
                obj.ShopTypeListings = ReadEntityList<CDataCommonU8>(buffer);
                obj.MainSuccessExample = ReadEntityList<CDataS2CEquipEnhancedGetPacksResUnk0Unk10>(buffer);
                return obj;
            }
        }
    }
}
