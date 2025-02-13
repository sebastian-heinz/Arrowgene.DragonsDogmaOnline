using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetAreaInfoListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_AREA_INFO_LIST_RES;

        public S2CQuestGetAreaInfoListRes()
        {
            AreaInfoList = new();
        }

        public List<CDataAreaInfoList> AreaInfoList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetAreaInfoListRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetAreaInfoListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.AreaInfoList);
            }

            public override S2CQuestGetAreaInfoListRes Read(IBuffer buffer)
            {
                S2CQuestGetAreaInfoListRes obj = new S2CQuestGetAreaInfoListRes();
                ReadServerResponse(buffer, obj);
                obj.AreaInfoList = ReadEntityList<CDataAreaInfoList>(buffer);
                return obj;
            }
        }
    }
}
