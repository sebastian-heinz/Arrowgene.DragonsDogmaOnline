using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBlackListGetBlackListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BLACK_LIST_GET_BLACK_LIST_RES;

        public List<CDataCommunityCharacterBaseInfo> BlackList { get; set; } = [];

        public class Serializer : PacketEntitySerializer<S2CBlackListGetBlackListRes>
        {
            public override void Write(IBuffer buffer, S2CBlackListGetBlackListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.BlackList);
            }

            public override S2CBlackListGetBlackListRes Read(IBuffer buffer)
            {
                S2CBlackListGetBlackListRes obj = new S2CBlackListGetBlackListRes();
                ReadServerResponse(buffer, obj);
                obj.BlackList = ReadEntityList<CDataCommunityCharacterBaseInfo>(buffer);
                return obj;
            }
        }
    }
}
