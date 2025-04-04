using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model
{
    public class CraftingRecipeGroup
    {
        public RecipeCategory Category { get; set; }
        public List<CraftingRecipe> RecipeList { get; set; } = new();
    }

    public class CraftingRecipe
    {
        public uint RecipeID { get; set; }
        public uint ItemID { get; set; }
        public uint Time { get; set; }
        public uint Num { get; set; }
        public uint Cost { get; set; }
        public uint Exp { get; set; }
        public uint NeedRank { get; set; }
        public int NpcAct { get; set; }
        public bool IsHide { get; set; }
        public List<CDataMDataCraftMaterial> CraftMaterialList { get; set; }
        public uint UnlockID { get; set; }

        public CDataMDataCraftRecipe AsCData()
        {
            return new CDataMDataCraftRecipe()
            {
                RecipeID = this.RecipeID,
                ItemID = this.ItemID,
                Time = this.Time,
                Num = this.Num,
                Cost = this.Cost,
                Exp = this.Exp,
                NeedRank = this.NeedRank,
                NpcAct = this.NpcAct,
                IsHide = this.IsHide,
                CraftMaterialList = this.CraftMaterialList,
            };
        }
    }
}
