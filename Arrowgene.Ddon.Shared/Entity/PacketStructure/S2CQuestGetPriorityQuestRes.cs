using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetPriorityQuestRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_PRIORITY_QUEST_RES;

        public S2CQuestGetPriorityQuestRes()
        {
            PriorityQuestSettingsList = new List<CDataPriorityQuestSetting>();
        }
        public List<CDataPriorityQuestSetting> PriorityQuestSettingsList {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetPriorityQuestRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetPriorityQuestRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataPriorityQuestSetting>(buffer, obj.PriorityQuestSettingsList);
            }

            public override S2CQuestGetPriorityQuestRes Read(IBuffer buffer)
            {
                S2CQuestGetPriorityQuestRes obj = new S2CQuestGetPriorityQuestRes();
                ReadServerResponse(buffer, obj);
                obj.PriorityQuestSettingsList = ReadEntityList<CDataPriorityQuestSetting>(buffer);
                return obj;
            }
        }
    }
}
