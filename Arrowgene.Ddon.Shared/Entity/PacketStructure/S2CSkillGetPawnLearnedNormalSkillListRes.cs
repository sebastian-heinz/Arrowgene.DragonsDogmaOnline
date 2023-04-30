using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillGetPawnLearnedNormalSkillListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_GET_PAWN_LEARNED_NORMAL_SKILL_LIST_RES;

        public S2CSkillGetPawnLearnedNormalSkillListRes()
        {
            NormalSkillParamList = new List<CDataNormalSkillParam>();
        }

        public uint PawnId { get; set; }
        public List<CDataNormalSkillParam> NormalSkillParamList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillGetPawnLearnedNormalSkillListRes>
        {
            public override void Write(IBuffer buffer, S2CSkillGetPawnLearnedNormalSkillListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList<CDataNormalSkillParam>(buffer, obj.NormalSkillParamList);
            }

            public override S2CSkillGetPawnLearnedNormalSkillListRes Read(IBuffer buffer)
            {
                S2CSkillGetPawnLearnedNormalSkillListRes obj = new S2CSkillGetPawnLearnedNormalSkillListRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.NormalSkillParamList = ReadEntityList<CDataNormalSkillParam>(buffer);
                return obj;
            }
        }
    }
}