using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPawnListData
    {
        public CDataPawnListData()
        {
            PawnCraftSkillList = new List<CDataPawnCraftSkill>();
        }

        public JobId Job { get; set; }
        public uint Level { get; set; }
        public uint CraftRank { get; set; }
        public List<CDataPawnCraftSkill> PawnCraftSkillList { get; set; }
        public uint CommentSize { get; set; }
        public ulong LatestReturnDate { get; set; }

        public class Serializer : EntitySerializer<CDataPawnListData>
        {
            public override void Write(IBuffer buffer, CDataPawnListData obj)
            {
                WriteByte(buffer, (byte) obj.Job);
                WriteUInt32(buffer, obj.Level);
                WriteUInt32(buffer, obj.CraftRank);
                WriteEntityList<CDataPawnCraftSkill>(buffer, obj.PawnCraftSkillList);
                WriteUInt32(buffer, obj.CommentSize);
                WriteUInt64(buffer, obj.LatestReturnDate);
            }

            public override CDataPawnListData Read(IBuffer buffer)
            {
                CDataPawnListData obj = new CDataPawnListData();
                obj.Job = (JobId) ReadByte(buffer);
                obj.Level = ReadUInt32(buffer);
                obj.CraftRank = ReadUInt32(buffer);
                obj.PawnCraftSkillList = ReadEntityList<CDataPawnCraftSkill>(buffer);
                obj.CommentSize = ReadUInt32(buffer);
                obj.LatestReturnDate = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}