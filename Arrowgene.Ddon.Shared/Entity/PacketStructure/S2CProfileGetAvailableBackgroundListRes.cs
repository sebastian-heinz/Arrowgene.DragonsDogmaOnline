using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CProfileGetAvailableBackgroundListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PROFILE_GET_AVAILABLE_BACKGROUND_LIST_RES;

        public List<CDataCommonU32> BackgroundIdList { get; set; } = new();
       
        public class Serializer : PacketEntitySerializer<S2CProfileGetAvailableBackgroundListRes>
        {
            public override void Write(IBuffer buffer, S2CProfileGetAvailableBackgroundListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataCommonU32>(buffer, obj.BackgroundIdList);
            }

            public override S2CProfileGetAvailableBackgroundListRes Read(IBuffer buffer)
            {
                S2CProfileGetAvailableBackgroundListRes obj = new S2CProfileGetAvailableBackgroundListRes();
                ReadServerResponse(buffer, obj);
                obj.BackgroundIdList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
