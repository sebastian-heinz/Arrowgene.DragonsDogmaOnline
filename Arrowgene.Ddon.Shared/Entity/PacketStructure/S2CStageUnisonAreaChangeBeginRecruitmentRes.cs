using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CStageUnisonAreaChangeBeginRecruitmentRes : ServerResponse
    {
        public S2CStageUnisonAreaChangeBeginRecruitmentRes()
        {
        }

        public override PacketId Id => PacketId.S2C_STAGE_UNISON_AREA_CHANGE_BEGIN_RECRUITMENT_RES;

        public class Serializer : PacketEntitySerializer<S2CStageUnisonAreaChangeBeginRecruitmentRes>
        {
            public override void Write(IBuffer buffer, S2CStageUnisonAreaChangeBeginRecruitmentRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CStageUnisonAreaChangeBeginRecruitmentRes Read(IBuffer buffer)
            {
                S2CStageUnisonAreaChangeBeginRecruitmentRes obj = new S2CStageUnisonAreaChangeBeginRecruitmentRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
