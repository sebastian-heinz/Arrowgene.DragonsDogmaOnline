namespace Arrowgene.Ddon.Shared.Model
{
    public enum CraftSkillType : byte
    {
        ProductionSpeed = 1,
        EquipmentEnhancement = 2,
        EquipmentQuality = 3,
        ConsumableQuantity = 4,
        CostPerformance = 5,

        /// shows up in analysis UI
        ConsumableProductionIsAlwaysGreatSuccess = 6,

        /// shows up in analysis UI
        CreatingHighQualityEquipmentIsAlwaysGreatSuccess = 7,

        /// shows up in analysis UI (competes with skill level and reports wrong value if this is set by itself, e.g. 10 > 5 instead of 10 > 2)
        CostPerformanceEffectUpFactor1 = 8,

        /// doesn't show up in analysis UI  (competes with skill level and reports wrong value if this is set by itself, e.g. 10 > 6 instead of 10 > 2)
        CostPerformanceEffectUpFactor2 = 9,
        
        /// Unknown effect currently
        UnknownEffect10 = 10
    }
}
