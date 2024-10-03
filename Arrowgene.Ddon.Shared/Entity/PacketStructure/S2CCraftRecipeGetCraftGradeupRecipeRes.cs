using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraftRecipeGetCraftGradeupRecipeRes : ServerResponse
    {

        public S2CCraftRecipeGetCraftGradeupRecipeRes()
        {
            RecipeList = new List<CDataMDataCraftGradeupRecipe>();
            UpgradableItemList = new List<CDataCommonU32>();
        }
        public override PacketId Id => PacketId.S2C_CRAFT_RECIPE_GET_CRAFT_GRADEUP_RECIPE_RES;

        public RecipeCategory Category { get; set; }
        public List<CDataMDataCraftGradeupRecipe> RecipeList { get; set; }
        public List<CDataCommonU32> UpgradableItemList { get; set; }
        public bool IsEnd { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCraftRecipeGetCraftGradeupRecipeRes>
        {
            public override void Write(IBuffer buffer, S2CCraftRecipeGetCraftGradeupRecipeRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, (byte)obj.Category);
                WriteEntityList<CDataMDataCraftGradeupRecipe>(buffer, obj.RecipeList);
                WriteEntityList<CDataCommonU32>(buffer, obj.UpgradableItemList);
                WriteBool(buffer, obj.IsEnd);
            }

            public override S2CCraftRecipeGetCraftGradeupRecipeRes Read(IBuffer buffer)
            {
                S2CCraftRecipeGetCraftGradeupRecipeRes obj = new S2CCraftRecipeGetCraftGradeupRecipeRes();
                ReadServerResponse(buffer, obj);
                obj.Category = (RecipeCategory)ReadByte(buffer);
                obj.RecipeList = ReadEntityList<CDataMDataCraftGradeupRecipe>(buffer);
                obj.UpgradableItemList = ReadEntityList<CDataCommonU32>(buffer);
                obj.IsEnd = ReadBool(buffer);
                return obj;
            }
        }
    }
}
