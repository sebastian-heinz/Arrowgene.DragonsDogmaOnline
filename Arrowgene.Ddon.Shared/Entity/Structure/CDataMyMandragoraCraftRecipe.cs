using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMyMandragoraCraftRecipe
    {
        public uint RecipeId { get; set; }
        public ItemId ItemId { get; set; }
        public uint Time { get; set; }

        /// <summary>
        /// TODO: Cost?
        /// </summary>
        public uint Unk3 { get; set; }

        /// <summary>
        /// TODO: ?
        /// </summary>
        public List<CDataMyMandragoraCraftRecipeUnk4> Unk4 { get; set; } = new();

        /// <summary>
        /// TODO: IsHide?
        /// </summary>
        public bool Unk5 { get; set; }

        public List<CDataMDataCraftMaterial> CraftMaterialList { get; set; } = new();

        public class Serializer : EntitySerializer<CDataMyMandragoraCraftRecipe>
        {
            public override void Write(IBuffer buffer, CDataMyMandragoraCraftRecipe obj)
            {
                WriteUInt32(buffer, obj.RecipeId);
                WriteUInt32(buffer, (uint)obj.ItemId);
                WriteUInt32(buffer, obj.Time);
                WriteUInt32(buffer, obj.Unk3);
                WriteEntityList<CDataMyMandragoraCraftRecipeUnk4>(buffer, obj.Unk4);
                WriteBool(buffer, obj.Unk5);
                WriteEntityList<CDataMDataCraftMaterial>(buffer, obj.CraftMaterialList);
            }

            public override CDataMyMandragoraCraftRecipe Read(IBuffer buffer)
            {
                CDataMyMandragoraCraftRecipe obj = new CDataMyMandragoraCraftRecipe();
                obj.RecipeId = ReadUInt32(buffer);
                obj.ItemId = (ItemId)ReadUInt32(buffer);
                obj.Time = ReadUInt32(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                obj.Unk4 = ReadEntityList<CDataMyMandragoraCraftRecipeUnk4>(buffer);
                obj.Unk5 = ReadBool(buffer);
                obj.CraftMaterialList = ReadEntityList<CDataMDataCraftMaterial>(buffer);
                return obj;
            }
        }
    }
}
