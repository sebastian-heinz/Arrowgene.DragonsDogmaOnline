using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CStageUnisonAreaChangeReadyCancelRes : ServerResponse
    {
        public S2CStageUnisonAreaChangeReadyCancelRes()
        {
        }

        public override PacketId Id => PacketId.S2C_STAGE_UNISON_AREA_CHANGE_READY_CANCEL_RES;

        public class Serializer : PacketEntitySerializer<S2CStageUnisonAreaChangeReadyCancelRes>
        {
            public override void Write(IBuffer buffer, S2CStageUnisonAreaChangeReadyCancelRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CStageUnisonAreaChangeReadyCancelRes Read(IBuffer buffer)
            {
                S2CStageUnisonAreaChangeReadyCancelRes obj = new S2CStageUnisonAreaChangeReadyCancelRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
