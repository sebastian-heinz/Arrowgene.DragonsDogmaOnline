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
        public uint Unk0 { get; set; } // this might be IR? 
        public uint GradeupItemID { get; set; }
        public uint Cost { get; set; }
        public uint Exp { get; set; }
        public bool Unk1 { get; set; }
        public List<CDataMDataCraftMaterial> CraftMaterialList { get; set; }

        public class Serializer : EntitySerializer<CDataMDataCraftGradeupRecipe>
        {
            public override void Write(IBuffer buffer, CDataMDataCraftGradeupRecipe obj)
            {
                WriteUInt32(buffer, obj.RecipeID);
                WriteUInt32(buffer, obj.ItemID);
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.GradeupItemID);
                WriteUInt32(buffer, obj.Cost);
                WriteUInt32(buffer, obj.Exp);
                WriteBool(buffer, obj.Unk1);
                WriteEntityList<CDataMDataCraftMaterial>(buffer, obj.CraftMaterialList);
            }

            public override CDataMDataCraftGradeupRecipe Read(IBuffer buffer)
            {
                    CDataMDataCraftGradeupRecipe obj = new CDataMDataCraftGradeupRecipe();
                    obj.RecipeID = ReadUInt32(buffer);
                    obj.ItemID = ReadUInt32(buffer);
                    obj.Unk0 = ReadUInt32(buffer);
                    obj.GradeupItemID = ReadUInt32(buffer);
                    obj.Cost = ReadUInt32(buffer);
                    obj.Exp = ReadUInt32(buffer);
                    obj.Unk1 = ReadBool(buffer);
                    obj.CraftMaterialList = ReadEntityList<CDataMDataCraftMaterial>(buffer);
                    return obj;
            }
        }
    }
}
