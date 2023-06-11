using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillLearnAbilityReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_LEARN_ABILITY_REQ;

        public JobId Job { get; set; }
        public uint AbilityId { get; set; }
        public byte AbilityLv { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSkillLearnAbilityReq>
        {
            public override void Write(IBuffer buffer, C2SSkillLearnAbilityReq obj)
            {
                WriteByte(buffer, (byte) obj.Job);
                WriteUInt32(buffer, obj.AbilityId);
                WriteByte(buffer, obj.AbilityLv);
            }

            public override C2SSkillLearnAbilityReq Read(IBuffer buffer)
            {
                C2SSkillLearnAbilityReq obj = new C2SSkillLearnAbilityReq();
                obj.Job = (JobId) ReadByte(buffer);
                obj.AbilityId = ReadUInt32(buffer);
                obj.AbilityLv = ReadByte(buffer);
                return obj;
            }
        }

    }
}