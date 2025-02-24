using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMandragoraGetCraftRecipeListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MANDRAGORA_GET_CRAFT_RECIPE_LIST_RES;

        /// <summary>
        /// TODO: category (dungeon ticket, materials)
        /// </summary>
        public byte Unk0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<CDataMyMandragoraCraftRecipe> MandragoraCraftRecipeList { get; set; }

        public S2CMandragoraGetCraftRecipeListRes()
        {
            MandragoraCraftRecipeList = new List<CDataMyMandragoraCraftRecipe>();
        }

        public class Serializer : PacketEntitySerializer<S2CMandragoraGetCraftRecipeListRes>
        {
            public override void Write(IBuffer buffer, S2CMandragoraGetCraftRecipeListRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteByte(buffer, obj.Unk0);
                WriteEntityList<CDataMyMandragoraCraftRecipe>(buffer, obj.MandragoraCraftRecipeList);
            }

            public override S2CMandragoraGetCraftRecipeListRes Read(IBuffer buffer)
            {
                S2CMandragoraGetCraftRecipeListRes obj = new S2CMandragoraGetCraftRecipeListRes();

                ReadServerResponse(buffer, obj);

                obj.Unk0 = ReadByte(buffer);
                obj.MandragoraCraftRecipeList = ReadEntityList<CDataMyMandragoraCraftRecipe>(buffer);

                return obj;
            }
        }
    }
}
