using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMandragoraBeginCraftReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MANDRAGORA_BEGIN_CRAFT_REQ;

        /// <summary>
        /// Recipe ID as provided via the previous GetCraftRecipeList request
        /// </summary>
        public uint RecipeId { get; set; }
        /// <summary>
        /// Mandragora chosen to perform crafting.
        /// </summary>
        public byte MandragoraId { get; set; }
        /// <summary>
        /// List of items used to "fertilize" or raise the mandragora.
        /// </summary>
        public List<CDataMyMandragoraBeginCraftFertilizerItem> CraftFertilizerItemList { get; set; } = new();
        /// <summary>
        /// Actual craft recipe ingredient items.
        /// </summary>
        public List<CDataCraftMaterial> CraftMaterialList { get; set; } = new();

        public C2SMandragoraBeginCraftReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SMandragoraBeginCraftReq>
        {
            public override void Write(IBuffer buffer, C2SMandragoraBeginCraftReq obj)
            {
                WriteUInt32(buffer, obj.RecipeId);
                WriteByte(buffer, obj.MandragoraId);
                WriteEntityList<CDataMyMandragoraBeginCraftFertilizerItem>(buffer, obj.CraftFertilizerItemList);
                WriteEntityList<CDataCraftMaterial>(buffer, obj.CraftMaterialList);
            }

            public override C2SMandragoraBeginCraftReq Read(IBuffer buffer)
            {
                C2SMandragoraBeginCraftReq obj = new C2SMandragoraBeginCraftReq();

                obj.RecipeId = ReadUInt32(buffer);
                obj.MandragoraId = ReadByte(buffer);
                obj.CraftFertilizerItemList = ReadEntityList<CDataMyMandragoraBeginCraftFertilizerItem>(buffer);
                obj.CraftMaterialList = ReadEntityList<CDataCraftMaterial>(buffer);

                return obj;
            }
        }
    }
}
