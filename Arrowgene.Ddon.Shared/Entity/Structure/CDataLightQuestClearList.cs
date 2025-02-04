using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLightQuestClearList
    {
        public CDataLightQuestClearList()
        {
        }

        public uint ScheduleId { get; set; }
        public uint ClearNum { get; set; }

        public class Serializer : EntitySerializer<CDataLightQuestClearList>
        {
            public override void Write(IBuffer buffer, CDataLightQuestClearList obj)
            {
                WriteUInt32(buffer, obj.ScheduleId);
                WriteUInt32(buffer, obj.ClearNum);
            }

            public override CDataLightQuestClearList Read(IBuffer buffer)
            {
                CDataLightQuestClearList obj = new CDataLightQuestClearList();
                obj.ScheduleId = ReadUInt32(buffer);
                obj.ClearNum = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}

