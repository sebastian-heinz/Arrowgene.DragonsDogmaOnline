using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetSetQuestInfoListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_SET_QUEST_INFO_LIST_RES;

        public S2CQuestGetSetQuestInfoListRes()
        {
            SetQuestList = new List<CDataSetQuestInfoList>();
        }

        public QuestAreaId DistributeId { get; set; }
        public List<CDataSetQuestInfoList> SetQuestList { get; set; }
        public ushort AreaBaseMinLevel { get; set; }
        public ushort AreaBaseMaxLevel { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetSetQuestInfoListRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetSetQuestInfoListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, (uint)obj.DistributeId);
                WriteEntityList<CDataSetQuestInfoList>(buffer, obj.SetQuestList);
                WriteUInt16(buffer, obj.AreaBaseMinLevel);
                WriteUInt16(buffer, obj.AreaBaseMaxLevel);
            }

            public override S2CQuestGetSetQuestInfoListRes Read(IBuffer buffer)
            {
                S2CQuestGetSetQuestInfoListRes obj = new S2CQuestGetSetQuestInfoListRes();
                ReadServerResponse(buffer, obj);
                obj.DistributeId = (QuestAreaId)ReadUInt32(buffer);
                obj.SetQuestList = ReadEntityList<CDataSetQuestInfoList>(buffer);
                obj.AreaBaseMinLevel = ReadUInt16(buffer);
                obj.AreaBaseMaxLevel = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
