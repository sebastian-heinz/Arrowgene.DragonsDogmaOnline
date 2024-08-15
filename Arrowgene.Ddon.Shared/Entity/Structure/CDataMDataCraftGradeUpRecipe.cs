using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMDataCraftGradeupRecipe
    {

        public CDataMDataCraftGradeupRecipe()
        {
            CraftMaterialList = new List<CDataMDataCraftMaterial>();
        }
        public uint RecipeID { get; set; }
        public uint ItemID { get; set; }
        public UpgradableStatus Upgradable { get; set; } // 1 is yes, 0 is no. will prompt a warning "will become max grade" 
        public uint GradeupItemID { get; set; }
        public uint Cost { get; set; }
        public uint Exp { get; set; }
        public bool AllowMultiGrade { get; set; } // Prompts a warning that you can't get multiple levels for this item.
        public List<CDataMDataCraftMaterial> CraftMaterialList { get; set; }

        public class Serializer : EntitySerializer<CDataMDataCraftGradeupRecipe>
        {
            public override void Write(IBuffer buffer, CDataMDataCraftGradeupRecipe obj)
            {
                WriteUInt32(buffer, obj.RecipeID);
                WriteUInt32(buffer, obj.ItemID);
                WriteUInt32(buffer, (uint)obj.Upgradable);
                WriteUInt32(buffer, obj.GradeupItemID);
                WriteUInt32(buffer, obj.Cost);
                WriteUInt32(buffer, obj.Exp);
                WriteBool(buffer, obj.AllowMultiGrade);
                WriteEntityList<CDataMDataCraftMaterial>(buffer, obj.CraftMaterialList);
            }

            public override CDataMDataCraftGradeupRecipe Read(IBuffer buffer)
            {
                    CDataMDataCraftGradeupRecipe obj = new CDataMDataCraftGradeupRecipe();
                    obj.RecipeID = ReadUInt32(buffer);
                    obj.ItemID = ReadUInt32(buffer);
                    obj.Upgradable = (UpgradableStatus)ReadUInt32(buffer); 
                    obj.GradeupItemID = ReadUInt32(buffer);
                    obj.Cost = ReadUInt32(buffer);
                    obj.Exp = ReadUInt32(buffer);
                    obj.AllowMultiGrade = ReadBool(buffer);
                    obj.CraftMaterialList = ReadEntityList<CDataMDataCraftMaterial>(buffer);
                    return obj;
            }
        }
    }
    public enum UpgradableStatus
    {
        No = 0,
        Yes = 1
    }
}
