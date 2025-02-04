using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CLoadingInfoLoadingGetInfoRes : ServerResponse
    {
        public S2CLoadingInfoLoadingGetInfoRes()
        {
            Info = new();
        }

        public override PacketId Id => PacketId.S2C_LOADING_INFO_LOADING_GET_INFO_RES;

        public List<CDataLoadingInfoSchedule> Info { get; set; }

        public class Serializer : PacketEntitySerializer<S2CLoadingInfoLoadingGetInfoRes>
        {
            public override void Write(IBuffer buffer, S2CLoadingInfoLoadingGetInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.Info);
            }

            public override S2CLoadingInfoLoadingGetInfoRes Read(IBuffer buffer)
            {
                S2CLoadingInfoLoadingGetInfoRes obj = new S2CLoadingInfoLoadingGetInfoRes();
                ReadServerResponse(buffer, obj);
                obj.Info = ReadEntityList<CDataLoadingInfoSchedule>(buffer);
                return obj;
            }
        }
    }
}
