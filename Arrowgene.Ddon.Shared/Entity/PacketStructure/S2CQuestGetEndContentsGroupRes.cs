using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetEndContentsGroupRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_END_CONTENTS_GROUP_RES;

        public S2CQuestGetEndContentsGroupRes()
        {
            TimeGainQuestList = new List<CDataTimeGainQuestList>();
        }

        public uint GroupId { get; set; }
        public ContentsType ContentsType {  get; set; }
        public List<CDataTimeGainQuestList> TimeGainQuestList {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetEndContentsGroupRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetEndContentsGroupRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.GroupId);
                WriteUInt32(buffer, (uint) obj.ContentsType);
                WriteEntityList(buffer, obj.TimeGainQuestList);
            }

            public override S2CQuestGetEndContentsGroupRes Read(IBuffer buffer)
            {
                S2CQuestGetEndContentsGroupRes obj = new S2CQuestGetEndContentsGroupRes();
                ReadServerResponse(buffer, obj);
                obj.GroupId = ReadUInt32(buffer);
                obj.ContentsType = (ContentsType) ReadUInt32(buffer);
                obj.TimeGainQuestList = ReadEntityList<CDataTimeGainQuestList>(buffer);
                return obj;
            }
        }
    }
}
