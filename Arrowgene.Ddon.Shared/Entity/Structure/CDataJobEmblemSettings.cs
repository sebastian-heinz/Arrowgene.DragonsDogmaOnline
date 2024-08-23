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
            EmblemCrestInheritenceBaseChanceList = new();
            InhertianceIncreaseChanceItemList = new();
            EmblemPointResetGGCostList = new();
            EmblemPointResetPPCostList = new();
            InheritencePremiumCurrencyCost = new();
            InventoryFilter = new();
        }

        public List<CDataJobEmblemLevelData> LevelingDataList { get; set; }
        public List<CDataJobEmblemStatCostData> StatCostList { get; set; } // Appears to define the amount the upgrades cost for stats
        public List<CDataJobEmblemStatUpgradeData> StatUpgradeDataList { get; set; } // Impacts the actual list
        public List<CDataJobEmblemSlotRestriction> CrestSlotRestrictionList { get; set; } // List appears to parse in reverse order in game UI
        public List<CDataJobEmblemCrestInheritenceBaseChance> EmblemCrestInheritenceBaseChanceList { get; set; } // Controls the display of the base chance %
        public List<CDataJobEmblemInhertianceIncreaseChanceItem> InhertianceIncreaseChanceItemList { get; set; } // Add information about chance in update screen
        public List<CDataJobEmblemActionCostParam> EmblemPointResetGGCostList { get; set; }
        public List<CDataJobEmblemActionCostParam> EmblemPointResetPPCostList { get; set; }
        public List<CDataJobEmblemActionCostParam> InheritencePremiumCurrencyCost { get; set; }
        public ushort MaxEmblemPoints { get; set; }
        public byte MaxEmblemLevel { get; set; }
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
                WriteEntityList(buffer, obj.EmblemCrestInheritenceBaseChanceList);
                WriteEntityList(buffer, obj.InhertianceIncreaseChanceItemList);
                WriteEntityList(buffer, obj.EmblemPointResetGGCostList);
                WriteEntityList(buffer, obj.EmblemPointResetPPCostList);
                WriteEntityList(buffer, obj.InheritencePremiumCurrencyCost);
                WriteUInt16(buffer, obj.MaxEmblemPoints);
                WriteByte(buffer, obj.MaxEmblemLevel);
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
                obj.EmblemCrestInheritenceBaseChanceList = ReadEntityList<CDataJobEmblemCrestInheritenceBaseChance>(buffer);
                obj.InhertianceIncreaseChanceItemList = ReadEntityList<CDataJobEmblemInhertianceIncreaseChanceItem>(buffer);
                obj.EmblemPointResetGGCostList = ReadEntityList<CDataJobEmblemActionCostParam>(buffer);
                obj.EmblemPointResetPPCostList = ReadEntityList<CDataJobEmblemActionCostParam>(buffer);
                obj.InheritencePremiumCurrencyCost = ReadEntityList<CDataJobEmblemActionCostParam>(buffer);
                obj.MaxEmblemPoints = ReadUInt16(buffer);
                obj.MaxEmblemLevel = ReadByte(buffer);
                obj.MaxInhertienceChanceIncrease = ReadByte(buffer);
                obj.InventoryFilter = ReadEntityList<CDataCommonU8>(buffer);
                return obj;
            }
        }
    }
}
