using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using System;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillLearnAbilityRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_LEARN_ABILITY_RES;

        public JobId Job { get; set; }
        public UInt32 NewJobPoint { get; set; }
        public UInt32 AbilityId { get; set; }
        public byte AbilityLv { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillLearnAbilityRes>
        {
            public override void Write(IBuffer buffer, S2CSkillLearnAbilityRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteByte(buffer, (byte) obj.Job);
                WriteUInt32(buffer, obj.NewJobPoint);
                WriteUInt32(buffer, obj.AbilityId);
                WriteByte(buffer, obj.AbilityLv);
            }

            public override S2CSkillLearnAbilityRes Read(IBuffer buffer)
            {
                S2CSkillLearnAbilityRes obj = new S2CSkillLearnAbilityRes();
                ReadServerResponse(buffer, obj);
                obj.Job = (JobId) ReadByte(buffer);
                obj.NewJobPoint = ReadUInt32(buffer);
                obj.AbilityId = ReadUInt32(buffer);
                obj.AbilityLv = ReadByte(buffer);
                return obj;
            }
        }
    }
}
