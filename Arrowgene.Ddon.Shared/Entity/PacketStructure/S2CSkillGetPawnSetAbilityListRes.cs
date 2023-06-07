using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillGetPawnSetAbilityListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_GET_PAWN_SET_ABILITY_LIST_RES;

        public S2CSkillGetPawnSetAbilityListRes()
        {
            SetAcquierementParamList = new List<CDataSetAcquirementParam>();
        }

        public uint PawnId { get; set; }
        public List<CDataSetAcquirementParam> SetAcquierementParamList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillGetPawnSetAbilityListRes>
        {
            public override void Write(IBuffer buffer, S2CSkillGetPawnSetAbilityListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList<CDataSetAcquirementParam>(buffer, obj.SetAcquierementParamList);
            }

            public override S2CSkillGetPawnSetAbilityListRes Read(IBuffer buffer)
            {
                S2CSkillGetPawnSetAbilityListRes obj = new S2CSkillGetPawnSetAbilityListRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.SetAcquierementParamList = ReadEntityList<CDataSetAcquirementParam>(buffer);
                return obj;
            }
        }
    }
}