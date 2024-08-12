using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillGetCharacterSkillInfoNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_SKILL_GET_CHARACTER_SKILL_INFO_NTC;

        public S2CSkillGetCharacterSkillInfoNtc()
        {
            NormalSkillList = new List<CDataNormalSkillParam>();
            SetCustomSkillList = new List<CDataSetAcquirementParam>();
            SetAbilityList = new List<CDataSetAcquirementParam>();
        }

        public uint CharacterId { get; set; }
        public JobId JobId { get; set; }
        public List<CDataNormalSkillParam> NormalSkillList { get; set; }
        public List<CDataSetAcquirementParam> SetCustomSkillList { get; set; }
        public List<CDataSetAcquirementParam> SetAbilityList { get; set; }
        public uint AbilityCostMax { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillGetCharacterSkillInfoNtc>
        {
            public override void Write(IBuffer buffer, S2CSkillGetCharacterSkillInfoNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteByte(buffer, (byte)obj.JobId);
                WriteEntityList<CDataNormalSkillParam>(buffer, obj.NormalSkillList);
                WriteEntityList<CDataSetAcquirementParam>(buffer, obj.SetCustomSkillList);
                WriteEntityList<CDataSetAcquirementParam>(buffer, obj.SetAbilityList);
                WriteUInt32(buffer, obj.AbilityCostMax);
            }

            public override S2CSkillGetCharacterSkillInfoNtc Read(IBuffer buffer)
            {
                S2CSkillGetCharacterSkillInfoNtc obj = new S2CSkillGetCharacterSkillInfoNtc();
                obj.CharacterId = ReadUInt32(buffer);
                obj.JobId = (JobId)ReadByte(buffer);
                obj.NormalSkillList = ReadEntityList<CDataNormalSkillParam>(buffer);
                obj.SetCustomSkillList = ReadEntityList<CDataSetAcquirementParam>(buffer);
                obj.SetAbilityList = ReadEntityList<CDataSetAcquirementParam>(buffer);
                obj.AbilityCostMax = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
