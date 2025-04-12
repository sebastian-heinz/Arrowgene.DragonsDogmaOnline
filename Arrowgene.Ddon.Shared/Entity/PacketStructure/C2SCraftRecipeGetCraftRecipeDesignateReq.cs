using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftRecipeGetCraftRecipeDesignateReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CRAFT_RECIPE_GET_CRAFT_RECIPE_DESIGNATE_REQ;

        public RecipeCategory Category { get; set; }
        public List<CDataCommonU32> ItemList { get; set; } = new();

        public class Serializer : PacketEntitySerializer<C2SCraftRecipeGetCraftRecipeDesignateReq>
        {
            public override void Write(IBuffer buffer, C2SCraftRecipeGetCraftRecipeDesignateReq obj)
            {
                WriteByte(buffer, (byte)obj.Category);
                WriteEntityList<CDataCommonU32>(buffer, obj.ItemList);
            }

            public override C2SCraftRecipeGetCraftRecipeDesignateReq Read(IBuffer buffer)
            {
                C2SCraftRecipeGetCraftRecipeDesignateReq obj = new C2SCraftRecipeGetCraftRecipeDesignateReq();
                obj.Category = (RecipeCategory)ReadByte(buffer);
                obj.ItemList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
