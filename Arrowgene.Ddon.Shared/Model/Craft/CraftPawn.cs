using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model.Craft
{
    public class CraftPawn
    {

        public Pawn Pawn { get; set; }
        public uint ProductionSpeed {  get; set; }
        public uint EquipmentEnhancement { get; set; }
        public uint EquipmentQuality { get; set; }
        public uint ConsumableQuantity {  get; set; }
        public uint CostPerformance { get; set; }

        public CraftPosition CraftPosition { get; set; }

        /// <summary>
        /// How much of their skill contributes based on their pawn type and position in the craft.
        /// See IR_COLLECTION_VALUE_LIST from the decompiled client.
        /// </summary>
        public double PositionModifier {  get; set; } 

        public CraftPawn(uint productionSpeed, uint equipmentEnhancement, uint equipmentQuality, uint consumableQuantity, uint costPerformance, CraftPosition craftPosition)
        {
            ProductionSpeed = productionSpeed;
            EquipmentEnhancement = equipmentEnhancement;
            EquipmentQuality = equipmentQuality;
            ConsumableQuantity = consumableQuantity;
            CostPerformance = costPerformance;
            CraftPosition = craftPosition;
            switch (craftPosition)
            {
                case CraftPosition.Leader:
                case CraftPosition.Legend:
                    PositionModifier = 1.0;
                    break;
                case CraftPosition.Assistant:
                    PositionModifier = 0.33;
                    break;
            }
        }

        public CraftPawn(Pawn pawn, CraftPosition position)
        {
            Pawn = pawn;
            ProductionSpeed = pawn.CraftData.PawnCraftSkillList.Find(skill => skill.Type == CraftSkillType.ProductionSpeed).Level;
            EquipmentEnhancement = pawn.CraftData.PawnCraftSkillList.Find(skill => skill.Type == CraftSkillType.EquipmentEnhancement).Level;
            EquipmentQuality = pawn.CraftData.PawnCraftSkillList.Find(skill => skill.Type == CraftSkillType.EquipmentQuality).Level;
            ConsumableQuantity = pawn.CraftData.PawnCraftSkillList.Find(skill => skill.Type == CraftSkillType.ConsumableQuantity).Level;
            CostPerformance = pawn.CraftData.PawnCraftSkillList.Find(skill => skill.Type == CraftSkillType.CostPerformance).Level;
            CraftPosition = position;
            switch (position)
            {
                case CraftPosition.Leader:
                    PositionModifier = 1.0;
                    break;
                case CraftPosition.Assistant:
                    PositionModifier = 0.33;
                    break;
            }
        }

        public CraftPawn(CDataRegisteredLegendPawnInfo legendPawn)
        {
            ProductionSpeed = legendPawn.PawnCraftSkillList.Find(skill => skill.Type == CraftSkillType.ProductionSpeed).Level;
            EquipmentEnhancement = legendPawn.PawnCraftSkillList.Find(skill => skill.Type == CraftSkillType.EquipmentEnhancement).Level;
            EquipmentQuality = legendPawn.PawnCraftSkillList.Find(skill => skill.Type == CraftSkillType.EquipmentQuality).Level;
            ConsumableQuantity = legendPawn.PawnCraftSkillList.Find(skill => skill.Type == CraftSkillType.ConsumableQuantity).Level;
            CostPerformance = legendPawn.PawnCraftSkillList.Find(skill => skill.Type == CraftSkillType.CostPerformance).Level;
            CraftPosition = CraftPosition.Legend;
            PositionModifier = 1.0;
        }
    }
}
