using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CStageGetSpAreaChangeIdFromNpcIdRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_STAGE_GET_SP_AREA_CHANGE_ID_FROM_NPC_ID_RES;

        public uint StageId { get; set; }

        public S2CStageGetSpAreaChangeIdFromNpcIdRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CStageGetSpAreaChangeIdFromNpcIdRes>
        {

            public override void Write(IBuffer buffer, S2CStageGetSpAreaChangeIdFromNpcIdRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.StageId);
            }

            public override S2CStageGetSpAreaChangeIdFromNpcIdRes Read(IBuffer buffer)
            {
                S2CStageGetSpAreaChangeIdFromNpcIdRes obj = new S2CStageGetSpAreaChangeIdFromNpcIdRes();
                ReadServerResponse(buffer, obj);
                obj.StageId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
