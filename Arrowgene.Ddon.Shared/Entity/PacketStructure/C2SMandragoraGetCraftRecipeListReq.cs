using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMandragoraGetCraftRecipeListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MANDRAGORA_GET_CRAFT_RECIPE_LIST_REQ;

        /// <summary>
        /// TODO: category (dungeon tickets, materials)?
        /// </summary>
        public byte Unk0 { get; set; }

        public byte MandragoraId { get; set; }

        public C2SMandragoraGetCraftRecipeListReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SMandragoraGetCraftRecipeListReq>
        {
            public override void Write(IBuffer buffer, C2SMandragoraGetCraftRecipeListReq obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteByte(buffer, obj.MandragoraId);
            }

            public override C2SMandragoraGetCraftRecipeListReq Read(IBuffer buffer)
            {
                C2SMandragoraGetCraftRecipeListReq obj = new C2SMandragoraGetCraftRecipeListReq();
                obj.Unk0 = ReadByte(buffer);
                obj.MandragoraId = ReadByte(buffer);
                return obj;
            }
        }
    }
}
