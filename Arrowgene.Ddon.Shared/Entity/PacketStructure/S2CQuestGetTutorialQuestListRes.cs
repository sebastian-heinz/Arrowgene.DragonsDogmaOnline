using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetTutorialQuestListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_TUTORIAL_QUEST_LIST_RES;

        public S2CQuestGetTutorialQuestListRes()
        {
            TutorialQuestList = new List<CDataTutorialQuestList>();
        }

        public uint StageNo {  get; set; }
        public List<CDataTutorialQuestList> TutorialQuestList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetTutorialQuestListRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetTutorialQuestListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.StageNo);
                WriteEntityList(buffer, obj.TutorialQuestList);
            }

            public override S2CQuestGetTutorialQuestListRes Read(IBuffer buffer)
            {
                S2CQuestGetTutorialQuestListRes obj = new S2CQuestGetTutorialQuestListRes();
                ReadServerResponse(buffer, obj);
                obj.StageNo = ReadUInt32(buffer);
                obj.TutorialQuestList = ReadEntityList<CDataTutorialQuestList>(buffer);
                return obj;
            }
        }
    }
}
