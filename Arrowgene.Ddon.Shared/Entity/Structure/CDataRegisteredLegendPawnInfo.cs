using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataRegisteredLegendPawnInfo
    {
        public CDataRegisteredLegendPawnInfo() {
            PawnCraftSkillList = new List<CDataPawnCraftSkill>();
        }
    
        public uint PawnId { get; set; }
        /// Although the structure supports changing this to any type, the in game UI only shows the GG icon
        public WalletType PointType { get; set; }
        /// how many GG or whatever else the point type is per craft usage 
        public uint RentalCost { get; set; }
        /// TODO: Maybe course ID?
        public uint Unk3 { get; set; }
        public string Name { get; set; } = string.Empty;
        public uint CraftRank { get; set; }
        public List<CDataPawnCraftSkill> PawnCraftSkillList { get; set; }
    
        public class Serializer : EntitySerializer<CDataRegisteredLegendPawnInfo>
        {
            public override void Write(IBuffer buffer, CDataRegisteredLegendPawnInfo obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, (byte)obj.PointType);
                WriteUInt32(buffer, obj.RentalCost);
                WriteUInt32(buffer, obj.Unk3);
                WriteMtString(buffer, obj.Name);
                WriteUInt32(buffer, obj.CraftRank);
                WriteEntityList<CDataPawnCraftSkill>(buffer, obj.PawnCraftSkillList);
            }
        
            public override CDataRegisteredLegendPawnInfo Read(IBuffer buffer)
            {
                CDataRegisteredLegendPawnInfo obj = new CDataRegisteredLegendPawnInfo();
                obj.PawnId = ReadUInt32(buffer);
                obj.PointType = (WalletType)ReadByte(buffer);
                obj.RentalCost = ReadUInt32(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                obj.Name = ReadMtString(buffer);
                obj.CraftRank = ReadUInt32(buffer);
                obj.PawnCraftSkillList = ReadEntityList<CDataPawnCraftSkill>(buffer);
                return obj;
            }
        }
    }
}
