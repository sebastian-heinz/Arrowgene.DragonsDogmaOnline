using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPriorityQuestSetting
    {
        public CDataPriorityQuestSetting()
        {
            PriorityQuestList = new List<CDataPriorityQuest>();
        }

        public uint CharacterId {  get; set; }
        public List<CDataPriorityQuest> PriorityQuestList { get; set; }

        public class Serializer : EntitySerializer<CDataPriorityQuestSetting>
        {
            public override void Write(IBuffer buffer, CDataPriorityQuestSetting obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteEntityList<CDataPriorityQuest>(buffer, obj.PriorityQuestList);
            }

            public override CDataPriorityQuestSetting Read(IBuffer buffer)
            {
                CDataPriorityQuestSetting obj = new CDataPriorityQuestSetting();
                obj.CharacterId = ReadUInt32(buffer);
                obj.PriorityQuestList = ReadEntityList<CDataPriorityQuest>(buffer);
                return obj;
            }
        }
    }
}
