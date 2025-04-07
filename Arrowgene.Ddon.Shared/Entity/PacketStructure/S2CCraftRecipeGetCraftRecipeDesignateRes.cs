using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraftRecipeGetCraftRecipeDesignateRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CRAFT_RECIPE_GET_CRAFT_RECIPE_DESIGNATE_RES;

        public RecipeCategory Category { get; set; }
        public List<CDataMDataCraftRecipe> RecipeList { get; set; } = new();
        public List<CDataCommonU32> ItemList { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CCraftRecipeGetCraftRecipeDesignateRes>
        {
            public override void Write(IBuffer buffer, S2CCraftRecipeGetCraftRecipeDesignateRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, (byte)obj.Category);
                WriteEntityList(buffer, obj.RecipeList);
                WriteEntityList(buffer, obj.ItemList);
            }

            public override S2CCraftRecipeGetCraftRecipeDesignateRes Read(IBuffer buffer)
            {
                S2CCraftRecipeGetCraftRecipeDesignateRes obj = new S2CCraftRecipeGetCraftRecipeDesignateRes();
                ReadServerResponse(buffer, obj);
                obj.Category = (RecipeCategory)ReadByte(buffer);
                obj.RecipeList = ReadEntityList<CDataMDataCraftRecipe>(buffer);
                obj.ItemList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
