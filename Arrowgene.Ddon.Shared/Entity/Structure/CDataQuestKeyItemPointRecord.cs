using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestKeyItemPointRecord
{
    public CDataQuestKeyItemPointRecord()
    {
        QuestKeyItemPointList = new List<CDataQuestKeyItemPoint>();
    }

    public uint QuestScheduleId { get; set; }
    public ushort ProcessNo { get; set; }
    public List<CDataQuestKeyItemPoint> QuestKeyItemPointList { get; set; }
    
    public class Serializer : EntitySerializer<CDataQuestKeyItemPointRecord>
    {
        public override void Write(IBuffer buffer, CDataQuestKeyItemPointRecord obj)
        {
            WriteUInt32(buffer, obj.QuestScheduleId);
            WriteUInt16(buffer, obj.ProcessNo);
            WriteEntityList<CDataQuestKeyItemPoint>(buffer, obj.QuestKeyItemPointList);
        }

        public override CDataQuestKeyItemPointRecord Read(IBuffer buffer)
        {
            CDataQuestKeyItemPointRecord obj = new CDataQuestKeyItemPointRecord();
            obj.QuestScheduleId = ReadUInt32(buffer);
            obj.ProcessNo = ReadUInt16(buffer);
            obj.QuestKeyItemPointList = ReadEntityList<CDataQuestKeyItemPoint>(buffer);
            return obj;
        }
    }
}
