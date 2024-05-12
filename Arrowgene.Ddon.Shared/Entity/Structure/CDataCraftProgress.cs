using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCraftProgress
    {
        public CDataCraftProgress() {
            CraftMainPawnInfo = new CDataCraftPawnInfo();
            CraftSupportPawnInfoList = new List<CDataCraftPawnInfo>();
            Unk0 = new List<CDataCommonU32>();
        }
    
        public CDataCraftPawnInfo CraftMainPawnInfo { get; set; }
        public List<CDataCraftPawnInfo> CraftSupportPawnInfoList { get; set; }
        public List<CDataCommonU32> Unk0 { get; set; }
        public uint RecipeId { get; set; }
        public uint Exp { get; set; }
        public int NpcActionId { get; set; }
        public uint ItemId { get; set; }
        public uint ToppingId { get; set; }
        public ushort Unk1 { get; set; }
        public uint RemainTime { get; set; }
        public bool ExpBonus { get; set; }
        public uint CreateCount { get; set; }
    
        public class Serializer : EntitySerializer<CDataCraftProgress>
        {
            public override void Write(IBuffer buffer, CDataCraftProgress obj)
            {
                WriteEntity<CDataCraftPawnInfo>(buffer, obj.CraftMainPawnInfo);
                WriteEntityList<CDataCraftPawnInfo>(buffer, obj.CraftSupportPawnInfoList);
                WriteEntityList<CDataCommonU32>(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.RecipeId);
                WriteUInt32(buffer, obj.Exp);
                WriteInt32(buffer, obj.NpcActionId);
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt32(buffer, obj.ToppingId);
                WriteUInt16(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.RemainTime);
                WriteBool(buffer, obj.ExpBonus);
                WriteUInt32(buffer, obj.CreateCount);
            }
        
            public override CDataCraftProgress Read(IBuffer buffer)
            {
                CDataCraftProgress obj = new CDataCraftProgress();
                obj.CraftMainPawnInfo = ReadEntity<CDataCraftPawnInfo>(buffer);
                obj.CraftSupportPawnInfoList = ReadEntityList<CDataCraftPawnInfo>(buffer);
                obj.Unk0 = ReadEntityList<CDataCommonU32>(buffer);
                obj.RecipeId = ReadUInt32(buffer);
                obj.Exp = ReadUInt32(buffer);
                obj.NpcActionId = ReadInt32(buffer);
                obj.ItemId = ReadUInt32(buffer);
                obj.ToppingId = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt16(buffer);
                obj.RemainTime = ReadUInt32(buffer);
                obj.ExpBonus = ReadBool(buffer);
                obj.CreateCount = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}