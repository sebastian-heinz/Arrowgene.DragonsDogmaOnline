using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPawnCraftData
    {
        public CDataPawnCraftData()
        {
            PawnCraftSkillList = new List<CDataPawnCraftSkill>();
        }

        public uint CraftExp { get; set; }
        public uint CraftRank { get; set; }
        public uint CraftRankLimit { get; set; }
        public uint CraftPoint { get; set; }
        public List<CDataPawnCraftSkill> PawnCraftSkillList { get; set; }

        public class Serializer : EntitySerializer<CDataPawnCraftData>
        {
            public override void Write(IBuffer buffer, CDataPawnCraftData obj)
            {
                WriteUInt32(buffer, obj.CraftExp);
                WriteUInt32(buffer, obj.CraftRank);
                WriteUInt32(buffer, obj.CraftRankLimit);
                WriteUInt32(buffer, obj.CraftPoint);
                WriteEntityList<CDataPawnCraftSkill>(buffer, obj.PawnCraftSkillList);
            }

            public override CDataPawnCraftData Read(IBuffer buffer)
            {
                CDataPawnCraftData obj = new CDataPawnCraftData();
                obj.CraftExp = ReadUInt32(buffer);
                obj.CraftRank = ReadUInt32(buffer);
                obj.CraftRankLimit = ReadUInt32(buffer);
                obj.CraftPoint = ReadUInt32(buffer);
                obj.PawnCraftSkillList = ReadEntityList<CDataPawnCraftSkill>(buffer);
                return obj;
            }
        }
    }
}
