using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobEmblemSettings
    {
        public CDataJobEmblemSettings()
        {
            LevelingDataList = new();
            StatCostList = new();
            StatUpgradeDataList = new();
            CrestSlotRestrictionList = new();
            EmblemCrestInheritanceBaseChanceList = new();
            InheritanceIncreaseChanceItemList = new();
            EmblemPointResetGGCostList = new();
            EmblemPointResetPPCostList = new();
            InheritancePremiumCurrencyCost = new();
            InventoryFilter = new();
        }

        public List<CDataJobEmblemLevelData> LevelingDataList { get; set; }
        public List<CDataJobEmblemStatCostData> StatCostList { get; set; } // Appears to define the amount the upgrades cost for stats
        public List<CDataJobEmblemStatUpgradeData> StatUpgradeDataList { get; set; } // Impacts the actual list
        public List<CDataJobEmblemSlotRestriction> CrestSlotRestrictionList { get; set; } // List appears to parse in reverse order in game UI
        public List<CDataJobEmblemCrestInheritanceBaseChance> EmblemCrestInheritanceBaseChanceList { get; set; } // Controls the display of the base chance %
        public List<CDataJobEmblemInhertianceIncreaseChanceItem> InheritanceIncreaseChanceItemList { get; set; } // Add information about chance in update screen
        public List<CDataJobEmblemActionCostParam> EmblemPointResetGGCostList { get; set; }
        public List<CDataJobEmblemActionCostParam> EmblemPointResetPPCostList { get; set; }
        public List<CDataJobEmblemActionCostParam> InheritancePremiumCurrencyCost { get; set; }
        public ushort MaxEmblemLevel { get; set; }
        public byte MaxEmblemStatUpgrades { get; set; }
        public byte MaxInhertienceChanceIncrease { get; set; }
        public List<CDataCommonU8> InventoryFilter { get; set; } // Controls the inventories that can be used (represents jewelry)

        public class Serializer : EntitySerializer<CDataJobEmblemSettings>
        {
            public override void Write(IBuffer buffer, CDataJobEmblemSettings obj)
            {
                WriteEntityList(buffer, obj.LevelingDataList);
                WriteEntityList(buffer, obj.StatCostList);
                WriteEntityList(buffer, obj.StatUpgradeDataList);
                WriteEntityList(buffer, obj.CrestSlotRestrictionList);
                WriteEntityList(buffer, obj.EmblemCrestInheritanceBaseChanceList);
                WriteEntityList(buffer, obj.InheritanceIncreaseChanceItemList);
                WriteEntityList(buffer, obj.EmblemPointResetGGCostList);
                WriteEntityList(buffer, obj.EmblemPointResetPPCostList);
                WriteEntityList(buffer, obj.InheritancePremiumCurrencyCost);
                WriteUInt16(buffer, obj.MaxEmblemLevel);
                WriteByte(buffer, obj.MaxEmblemStatUpgrades);
                WriteByte(buffer, obj.MaxInhertienceChanceIncrease);
                WriteEntityList(buffer, obj.InventoryFilter);
            }

            public override CDataJobEmblemSettings Read(IBuffer buffer)
            {
                CDataJobEmblemSettings obj = new CDataJobEmblemSettings();
                obj.LevelingDataList = ReadEntityList<CDataJobEmblemLevelData>(buffer);
                obj.StatCostList = ReadEntityList<CDataJobEmblemStatCostData>(buffer);
                obj.StatUpgradeDataList = ReadEntityList<CDataJobEmblemStatUpgradeData>(buffer);
                obj.CrestSlotRestrictionList = ReadEntityList<CDataJobEmblemSlotRestriction>(buffer);
                obj.EmblemCrestInheritanceBaseChanceList = ReadEntityList<CDataJobEmblemCrestInheritanceBaseChance>(buffer);
                obj.InheritanceIncreaseChanceItemList = ReadEntityList<CDataJobEmblemInhertianceIncreaseChanceItem>(buffer);
                obj.EmblemPointResetGGCostList = ReadEntityList<CDataJobEmblemActionCostParam>(buffer);
                obj.EmblemPointResetPPCostList = ReadEntityList<CDataJobEmblemActionCostParam>(buffer);
                obj.InheritancePremiumCurrencyCost = ReadEntityList<CDataJobEmblemActionCostParam>(buffer);
                obj.MaxEmblemLevel = ReadUInt16(buffer);
                obj.MaxEmblemStatUpgrades = ReadByte(buffer);
                obj.MaxInhertienceChanceIncrease = ReadByte(buffer);
                obj.InventoryFilter = ReadEntityList<CDataCommonU8>(buffer);
                return obj;
            }
        }
    }
}
