using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAreaBaseInfo
    {
        public QuestAreaId AreaID { get; set; }
        public uint Rank { get; set; }
        public uint CurrentPoint { get; set; }
        public uint NextPoint { get; set; }
        public uint WeekPoint { get; set; }
        public bool CanRankUp { get; set; }
        public uint ClanAreaPoint { get; set; }
        public uint ClanAreaPointBorder { get; set; }
        public bool CanReceiveSupply { get; set; }
    
        public class Serializer : EntitySerializer<CDataAreaBaseInfo>
        {
            public override void Write(IBuffer buffer, CDataAreaBaseInfo obj)
            {
                WriteUInt32(buffer, (uint)obj.AreaID);
                WriteUInt32(buffer, obj.Rank);
                WriteUInt32(buffer, obj.CurrentPoint);
                WriteUInt32(buffer, obj.NextPoint);
                WriteUInt32(buffer, obj.WeekPoint);
                WriteBool(buffer, obj.CanRankUp);
                WriteUInt32(buffer, obj.ClanAreaPoint);
                WriteUInt32(buffer, obj.ClanAreaPointBorder);
                WriteBool(buffer, obj.CanReceiveSupply);
            }
        
            public override CDataAreaBaseInfo Read(IBuffer buffer)
            {
                CDataAreaBaseInfo obj = new CDataAreaBaseInfo();
                obj.AreaID = (QuestAreaId)ReadUInt32(buffer);
                obj.Rank = ReadUInt32(buffer);
                obj.CurrentPoint = ReadUInt32(buffer);
                obj.NextPoint = ReadUInt32(buffer);
                obj.WeekPoint = ReadUInt32(buffer);
                obj.CanRankUp = ReadBool(buffer);
                obj.ClanAreaPoint = ReadUInt32(buffer);
                obj.ClanAreaPointBorder = ReadUInt32(buffer);
                obj.CanReceiveSupply = ReadBool(buffer);
                return obj;
            }
        }
    }
}
