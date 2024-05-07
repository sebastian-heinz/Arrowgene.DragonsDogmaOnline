using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetMainQuestListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_MAIN_QUEST_LIST_RES;

        public S2CQuestGetMainQuestListRes()
        {
            MainQuestList = new List<CDataQuestList>();
        }

        public List<CDataQuestList> MainQuestList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetMainQuestListRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetMainQuestListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataQuestList>(buffer, obj.MainQuestList);
            }

            public override S2CQuestGetMainQuestListRes Read(IBuffer buffer)
            {
                S2CQuestGetMainQuestListRes obj = new S2CQuestGetMainQuestListRes();
                ReadServerResponse(buffer, obj);
                obj.MainQuestList = ReadEntityList<CDataQuestList>(buffer);
                return obj;
            }
        }
    }
}
