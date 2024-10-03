using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftRecipeGetCraftGradeupRecipeReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CRAFT_RECIPE_GET_CRAFT_GRADEUP_RECIPE_REQ;

        public RecipeCategory Category { get; set; }
        public uint Offset { get; set; }
        public int Num { get; set; }
        public List<CDataCommonU32> ItemList { get; set; }

        public C2SCraftRecipeGetCraftGradeupRecipeReq()
        {
            ItemList = new List<CDataCommonU32>();
        }

        public class Serializer : PacketEntitySerializer<C2SCraftRecipeGetCraftGradeupRecipeReq>
        {
            public override void Write(IBuffer buffer, C2SCraftRecipeGetCraftGradeupRecipeReq obj)
            {
                WriteByte(buffer, (byte)obj.Category);
                WriteUInt32(buffer, obj.Offset);
                WriteInt32(buffer, obj.Num);
                WriteEntityList<CDataCommonU32>(buffer, obj.ItemList);
            }

            public override C2SCraftRecipeGetCraftGradeupRecipeReq Read(IBuffer buffer)
            {
                C2SCraftRecipeGetCraftGradeupRecipeReq obj = new C2SCraftRecipeGetCraftGradeupRecipeReq();
                obj.Category = (RecipeCategory)ReadByte(buffer);
                obj.Offset = ReadUInt32(buffer);
                obj.Num = ReadInt32(buffer);
                obj.ItemList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
