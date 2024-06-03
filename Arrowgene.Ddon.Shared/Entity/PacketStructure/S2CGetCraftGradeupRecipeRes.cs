using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGetCraftGradeupRecipeRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CRAFT_RECIPE_GET_CRAFT_GRADEUP_RECIPE_RES;

        public byte Category { get; set; }
        public List<CDataMDataCraftGradeupRecipe> RecipeList { get; set; }
        public S2CGetCraftGradeupRecipeResUnk0 Unk0 { get; set; }
        public List<CDataCommonU32> UnknownItemList { get; set; }
        public bool IsEnd { get; set; }

        public S2CGetCraftGradeupRecipeRes()
        {
            RecipeList = new List<CDataMDataCraftGradeupRecipe>();
            Unk0 = new S2CGetCraftGradeupRecipeResUnk0();
            UnknownItemList = new List<CDataCommonU32>();
        }

        public class Serializer : PacketEntitySerializer<S2CGetCraftGradeupRecipeRes>
        {
            public override void Write(IBuffer buffer, S2CGetCraftGradeupRecipeRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, obj.Category);
                WriteEntityList<CDataMDataCraftGradeupRecipe>(buffer, obj.RecipeList);
                WriteEntity<S2CGetCraftGradeupRecipeResUnk0>(buffer, obj.Unk0);
                WriteEntityList<CDataCommonU32>(buffer, obj.UnknownItemList);
                WriteBool(buffer, obj.IsEnd);
            }

            public override S2CGetCraftGradeupRecipeRes Read(IBuffer buffer)
            {
                S2CGetCraftGradeupRecipeRes obj = new S2CGetCraftGradeupRecipeRes();
                ReadServerResponse(buffer, obj);
                obj.Category = ReadByte(buffer);
                obj.RecipeList = ReadEntityList<CDataMDataCraftGradeupRecipe>(buffer);
                obj.Unk0 = ReadEntity<S2CGetCraftGradeupRecipeResUnk0>(buffer);
                obj.UnknownItemList = ReadEntityList<CDataCommonU32>(buffer);
                obj.IsEnd = ReadBool(buffer);
                return obj;
            }
        }
    }
}
