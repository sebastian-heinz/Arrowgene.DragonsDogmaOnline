using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetCycleContentsStateListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_CYCLE_CONTENTS_STATE_LIST_RES;

        public S2CQuestGetCycleContentsStateListRes()
        {
            CycleContentsStateList = new();
        }

        public List<CDataCycleContentsStateList> CycleContentsStateList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetCycleContentsStateListRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetCycleContentsStateListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataCycleContentsStateList>(buffer, obj.CycleContentsStateList);
            }

            public override S2CQuestGetCycleContentsStateListRes Read(IBuffer buffer)
            {
                S2CQuestGetCycleContentsStateListRes obj = new S2CQuestGetCycleContentsStateListRes();
                ReadServerResponse(buffer, obj);
                obj.CycleContentsStateList = ReadEntityList<CDataCycleContentsStateList>(buffer);
                return obj;
            }
        }
    }
}
