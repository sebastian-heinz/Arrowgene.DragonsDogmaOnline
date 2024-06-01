using System.Linq;
using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMDataCraftGradeupRecipe
    {
        public uint RecipeID { get; set; }
        public uint ItemID { get; set; }
        public uint GradeupItemID { get; set; }
        public uint Cost { get; set; }
        public uint Exp { get; set; }
        public List<CDataMDataCraftMaterial> CraftMaterialList { get; set; }

        public CDataMDataCraftGradeupRecipe()
        {
            CraftMaterialList = new List<CDataMDataCraftMaterial>();
        }

        public class Serializer : EntitySerializer<CDataMDataCraftGradeupRecipe>
        {
            public override void Write(IBuffer buffer, CDataMDataCraftGradeupRecipe obj)
            {
                WriteUInt32(buffer, obj.RecipeID);
                WriteUInt32(buffer, obj.ItemID);
                WriteUInt32(buffer, obj.GradeupItemID);
                WriteUInt32(buffer, obj.Cost);
                WriteUInt32(buffer, obj.Exp);
                WriteEntityList<CDataMDataCraftMaterial>(buffer, obj.CraftMaterialList);
            }

            public override CDataMDataCraftGradeupRecipe Read(IBuffer buffer)
            {
                CDataMDataCraftGradeupRecipe obj = new CDataMDataCraftGradeupRecipe
                {
                    RecipeID = buffer.ReadUInt32(),
                    ItemID = buffer.ReadUInt32(),
                    GradeupItemID = buffer.ReadUInt32(),
                    Cost = buffer.ReadUInt32(),
                    Exp = buffer.ReadUInt32()
                };

                int materialCount = buffer.ReadInt32();
                for (int i = 0; i < materialCount; i++)
                {
                    // Read each material individually
                    CDataMDataCraftMaterial material = new CDataMDataCraftMaterial
                    {
                        ItemId = buffer.ReadUInt32(),
                        Num = buffer.ReadUInt32(),
                        SortNo = buffer.ReadUInt32(),
                        IsSp = buffer.ReadBool()
                    };
                    obj.CraftMaterialList.Add(material);
                }

                return obj;
            }
        }
    }
}
