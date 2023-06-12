using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSkillLearnPawnAbilityRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SKILL_LEARN_PAWN_ABILITY_RES;

        public uint PawnId { get; set; }
        public JobId Job { get; set; }
        public uint NewJobPoint { get; set; }
        public uint AbilityId { get; set; }
        public byte AbilityLv { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSkillLearnPawnAbilityRes>
        {
            public override void Write(IBuffer buffer, S2CSkillLearnPawnAbilityRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, (byte) obj.Job);
                WriteUInt32(buffer, obj.NewJobPoint);
                WriteUInt32(buffer, obj.AbilityId);
                WriteByte(buffer, obj.AbilityLv);                
            }

            public override S2CSkillLearnPawnAbilityRes Read(IBuffer buffer)
            {
                S2CSkillLearnPawnAbilityRes obj = new S2CSkillLearnPawnAbilityRes();
                ReadServerResponse(buffer, obj);
                obj.PawnId = ReadUInt32(buffer);
                obj.Job = (JobId) ReadByte(buffer);
                obj.NewJobPoint = ReadUInt32(buffer);
                obj.AbilityId = ReadUInt32(buffer);
                obj.AbilityLv = ReadByte(buffer);                
                return obj;
            }
        }
    }
}