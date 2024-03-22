using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMDataCraftRecipe
    {
        public CDataMDataCraftRecipe() {
            CraftMaterialList = new List<CDataMDataCraftMaterial>();
        }
    
        public uint RecipeID { get; set; }
        public uint ItemID { get; set; }
        public uint Time { get; set; }
        public uint Num { get; set; }
        public uint Cost { get; set; }
        public uint Exp { get; set; }
        public uint NeedRank { get; set; }
        public int NpcAct { get; set; }
        public bool IsHide { get; set; }
        public List<CDataMDataCraftMaterial> CraftMaterialList { get; set; }
    
        public class Serializer : EntitySerializer<CDataMDataCraftRecipe>
        {
            public override void Write(IBuffer buffer, CDataMDataCraftRecipe obj)
            {
                WriteUInt32(buffer, obj.RecipeID);
                WriteUInt32(buffer, obj.ItemID);
                WriteUInt32(buffer, obj.Time);
                WriteUInt32(buffer, obj.Num);
                WriteUInt32(buffer, obj.Cost);
                WriteUInt32(buffer, obj.Exp);
                WriteUInt32(buffer, obj.NeedRank);
                WriteInt32(buffer, obj.NpcAct);
                WriteBool(buffer, obj.IsHide);
                WriteEntityList<CDataMDataCraftMaterial>(buffer, obj.CraftMaterialList);
            }
        
            public override CDataMDataCraftRecipe Read(IBuffer buffer)
            {
                CDataMDataCraftRecipe obj = new CDataMDataCraftRecipe();
                obj.RecipeID = ReadUInt32(buffer);
                obj.ItemID = ReadUInt32(buffer);
                obj.Time = ReadUInt32(buffer);
                obj.Num = ReadUInt32(buffer);
                obj.Cost = ReadUInt32(buffer);
                obj.Exp = ReadUInt32(buffer);
                obj.NeedRank = ReadUInt32(buffer);
                obj.NpcAct = ReadInt32(buffer);
                obj.IsHide = ReadBool(buffer);
                obj.CraftMaterialList = ReadEntityList<CDataMDataCraftMaterial>(buffer);
                return obj;
            }
        }
    }
}