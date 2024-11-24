using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SStageUnisonAreaChangeGetRecruitmentStateReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_STAGE_UNISON_AREA_CHANGE_GET_RECRUITMENT_STATE_REQ;

        public C2SStageUnisonAreaChangeGetRecruitmentStateReq()
        {
        }

        public uint DungeonId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SStageUnisonAreaChangeGetRecruitmentStateReq>
        {
            public override void Write(IBuffer buffer, C2SStageUnisonAreaChangeGetRecruitmentStateReq obj)
            {
                WriteUInt32(buffer, obj.DungeonId);
            }

            public override C2SStageUnisonAreaChangeGetRecruitmentStateReq Read(IBuffer buffer)
            {
                C2SStageUnisonAreaChangeGetRecruitmentStateReq obj = new C2SStageUnisonAreaChangeGetRecruitmentStateReq();
                obj.DungeonId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
