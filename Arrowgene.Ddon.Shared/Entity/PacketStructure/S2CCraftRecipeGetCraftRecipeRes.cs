using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraftRecipeGetCraftRecipeRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CRAFT_RECIPE_GET_CRAFT_RECIPE_RES;

        public S2CCraftRecipeGetCraftRecipeRes()
        {
            RecipeList = new List<CDataMDataCraftRecipe>();
        }

        public RecipeCategory Category { get; set; }
        public List<CDataMDataCraftRecipe> RecipeList { get; set; }
        public bool IsEnd { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCraftRecipeGetCraftRecipeRes>
        {
            public override void Write(IBuffer buffer, S2CCraftRecipeGetCraftRecipeRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, (byte)obj.Category);
                WriteEntityList<CDataMDataCraftRecipe>(buffer, obj.RecipeList);
                WriteBool(buffer, obj.IsEnd);
            }

            public override S2CCraftRecipeGetCraftRecipeRes Read(IBuffer buffer)
            {
                S2CCraftRecipeGetCraftRecipeRes obj = new S2CCraftRecipeGetCraftRecipeRes();
                ReadServerResponse(buffer, obj);
                obj.Category = (RecipeCategory)ReadByte(buffer);
                obj.RecipeList = ReadEntityList<CDataMDataCraftRecipe>(buffer);
                obj.IsEnd = ReadBool(buffer);
                return obj;
            }
        }
    }
}
