using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CAreaGetAreaBaseInfoListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_AREA_GET_AREA_BASE_INFO_LIST_RES;

        public S2CAreaGetAreaBaseInfoListRes()
        {
            AreaBaseInfoList = new List<CDataAreaBaseInfo>();
        }

        public List<CDataAreaBaseInfo> AreaBaseInfoList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CAreaGetAreaBaseInfoListRes>
        {
            public override void Write(IBuffer buffer, S2CAreaGetAreaBaseInfoListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataAreaBaseInfo>(buffer, obj.AreaBaseInfoList);
            }

            public override S2CAreaGetAreaBaseInfoListRes Read(IBuffer buffer)
            {
                S2CAreaGetAreaBaseInfoListRes obj = new S2CAreaGetAreaBaseInfoListRes();
                ReadServerResponse(buffer, obj);
                obj.AreaBaseInfoList = ReadEntityList<CDataAreaBaseInfo>(buffer);
                return obj;
            }
        }
    }
}
