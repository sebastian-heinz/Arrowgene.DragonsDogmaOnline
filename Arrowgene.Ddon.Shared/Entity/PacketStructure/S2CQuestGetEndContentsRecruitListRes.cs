using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetEndContentsRecruitListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_END_CONTENTS_RECRUIT_LIST_RES;

        public S2CQuestGetEndContentsRecruitListRes()
        {
            Unk1List = new List<CDataQuestRecruitListItem>();
        }

        public uint Unk0 {  get; set; }
        public List<CDataQuestRecruitListItem> Unk1List {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetEndContentsRecruitListRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetEndContentsRecruitListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.Unk0);
                WriteEntityList(buffer, obj.Unk1List);
            }

            public override S2CQuestGetEndContentsRecruitListRes Read(IBuffer buffer)
            {
                S2CQuestGetEndContentsRecruitListRes obj = new S2CQuestGetEndContentsRecruitListRes();
                ReadServerResponse(buffer, obj);
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1List = ReadEntityList<CDataQuestRecruitListItem>(buffer);
                return obj;
            }
        }
    }
}

