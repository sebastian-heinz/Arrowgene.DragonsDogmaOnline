using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftRecipeGetCraftRecipeReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CRAFT_RECIPE_GET_CRAFT_RECIPE_REQ;

        public RecipeCategory Category { get; set; }
        public uint Offset { get; set; }
        public int Num { get; set; }

        public class Serializer : PacketEntitySerializer<C2SCraftRecipeGetCraftRecipeReq>
        {
            public override void Write(IBuffer buffer, C2SCraftRecipeGetCraftRecipeReq obj)
            {
                WriteByte(buffer, (byte)obj.Category);
                WriteUInt32(buffer, obj.Offset);
                WriteInt32(buffer, obj.Num);
            }

            public override C2SCraftRecipeGetCraftRecipeReq Read(IBuffer buffer)
            {
                C2SCraftRecipeGetCraftRecipeReq obj = new C2SCraftRecipeGetCraftRecipeReq();
                obj.Category = (RecipeCategory)ReadByte(buffer);
                obj.Offset = ReadUInt32(buffer);
                obj.Num = ReadInt32(buffer);
                return obj;
            }
        }

    }
}
