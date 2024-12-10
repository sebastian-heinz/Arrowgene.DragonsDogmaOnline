using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CStageUnisonAreaChangeReadyRes : ServerResponse
    {
        public S2CStageUnisonAreaChangeReadyRes()
        {
        }

        public override PacketId Id => PacketId.S2C_STAGE_UNISON_AREA_CHANGE_READY_RES;

        public class Serializer : PacketEntitySerializer<S2CStageUnisonAreaChangeReadyRes>
        {
            public override void Write(IBuffer buffer, S2CStageUnisonAreaChangeReadyRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CStageUnisonAreaChangeReadyRes Read(IBuffer buffer)
            {
                S2CStageUnisonAreaChangeReadyRes obj = new S2CStageUnisonAreaChangeReadyRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
