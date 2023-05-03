using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillGetPawnSetSkillListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_GET_PAWN_SET_SKILL_LIST_RES;

        public S2CSkillGetPawnSetSkillListRes()
        {
            SetAcquierementParamList = new List<CDataSetAcquirementParam>();
        }

        public uint PawnId { get; set; }
        public List<CDataSetAcquirementParam> SetAcquierementParamList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillGetPawnSetSkillListRes>
        {
            public override void Write(IBuffer buffer, S2CSkillGetPawnSetSkillListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList<CDataSetAcquirementParam>(buffer, obj.SetAcquierementParamList);
            }

            public override S2CSkillGetPawnSetSkillListRes Read(IBuffer buffer)
            {
                S2CSkillGetPawnSetSkillListRes obj = new S2CSkillGetPawnSetSkillListRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.SetAcquierementParamList = ReadEntityList<CDataSetAcquirementParam>(buffer);
                return obj;
            }
        }
    }
}