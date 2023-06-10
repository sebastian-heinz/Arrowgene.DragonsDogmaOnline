using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillGetPawnLearnedAbilityListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_GET_PAWN_LEARNED_ABILITY_LIST_RES;

        public S2CSkillGetPawnLearnedAbilityListRes()
        {
            SetAcquierementParam=new List<CDataLearnedSetAcquirementParam>();
        }

        public uint PawnId { get; set; }
        public List<CDataLearnedSetAcquirementParam> SetAcquierementParam { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillGetPawnLearnedAbilityListRes>
        {
            public override void Write(IBuffer buffer, S2CSkillGetPawnLearnedAbilityListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteEntityList<CDataLearnedSetAcquirementParam>(buffer, obj.SetAcquierementParam);
            }

            public override S2CSkillGetPawnLearnedAbilityListRes Read(IBuffer buffer)
            {
                S2CSkillGetPawnLearnedAbilityListRes obj = new S2CSkillGetPawnLearnedAbilityListRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.SetAcquierementParam = ReadEntityList<CDataLearnedSetAcquirementParam>(buffer);
                return obj;
            }
        }
    }
}