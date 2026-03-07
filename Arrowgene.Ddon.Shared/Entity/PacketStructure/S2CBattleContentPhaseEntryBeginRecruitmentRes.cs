using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentPhaseEntryBeginRecruitmentRes : ServerResponse
    {
        public S2CBattleContentPhaseEntryBeginRecruitmentRes()
        {
        }

        public override PacketId Id => PacketId.S2C_BATTLE_CONTENT_PHASE_ENTRY_BEGIN_RECRUITMENT_RES;

        public class Serializer : PacketEntitySerializer<S2CBattleContentPhaseEntryBeginRecruitmentRes>
        {
            public override void Write(IBuffer buffer, S2CBattleContentPhaseEntryBeginRecruitmentRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CBattleContentPhaseEntryBeginRecruitmentRes Read(IBuffer buffer)
            {
                S2CBattleContentPhaseEntryBeginRecruitmentRes obj = new();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
