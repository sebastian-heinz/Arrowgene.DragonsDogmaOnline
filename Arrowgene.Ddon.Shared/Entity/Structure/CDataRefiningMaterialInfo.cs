using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataRefiningMaterialInfo
    {
        public uint ItemId { get; set; }
        public uint CraftCostItemRankMultiplier { get; set; }
        public uint CraftCostMax { get; set; }
        public uint CraftExpItemRankMultiplier { get; set; }
        public uint CraftExpMax { get; set; }
        public bool CanGreatSuccess { get; set; }
    
        public class Serializer : EntitySerializer<CDataRefiningMaterialInfo>
        {
            public override void Write(IBuffer buffer, CDataRefiningMaterialInfo obj)
            {
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt32(buffer, obj.CraftCostItemRankMultiplier);
                WriteUInt32(buffer, obj.CraftCostMax);
                WriteUInt32(buffer, obj.CraftExpItemRankMultiplier);
                WriteUInt32(buffer, obj.CraftExpMax);
                WriteBool(buffer, obj.CanGreatSuccess);
            }
        
            public override CDataRefiningMaterialInfo Read(IBuffer buffer)
            {
                CDataRefiningMaterialInfo obj = new CDataRefiningMaterialInfo();
                obj.ItemId = ReadUInt32(buffer);
                obj.CraftCostItemRankMultiplier = ReadUInt32(buffer);
                obj.CraftCostMax = ReadUInt32(buffer);
                obj.CraftExpItemRankMultiplier = ReadUInt32(buffer);
                obj.CraftExpMax = ReadUInt32(buffer);
                obj.CanGreatSuccess = ReadBool(buffer);
                return obj;
            }
        }
    }
}
