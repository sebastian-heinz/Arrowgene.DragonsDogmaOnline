using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillGetPawnLearnedSkillListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_GET_PAWN_LEARNED_SKILL_LIST_RES;

        public S2CSkillGetPawnLearnedSkillListRes()
        {
            LearnedAcquirementParamList = new List<CDataLearnedSetAcquirementParam>();
        }

        public uint PawnId { get; set; }
        public List<CDataLearnedSetAcquirementParam> LearnedAcquirementParamList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillGetPawnLearnedSkillListRes>
        {
            public override void Write(IBuffer buffer, S2CSkillGetPawnLearnedSkillListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList<CDataLearnedSetAcquirementParam>(buffer, obj.LearnedAcquirementParamList);
            }

            public override S2CSkillGetPawnLearnedSkillListRes Read(IBuffer buffer)
            {
                S2CSkillGetPawnLearnedSkillListRes obj = new S2CSkillGetPawnLearnedSkillListRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.LearnedAcquirementParamList = ReadEntityList<CDataLearnedSetAcquirementParam>(buffer);
                return obj;
            }
        }
    }
}
