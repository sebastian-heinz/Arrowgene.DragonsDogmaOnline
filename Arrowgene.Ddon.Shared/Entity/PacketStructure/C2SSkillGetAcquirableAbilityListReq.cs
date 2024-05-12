using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillGetAcquirableAbilityListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_GET_ACQUIRABLE_ABILITY_LIST_REQ;

        public uint CharacterId { get; set; }
        public JobId Job { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSkillGetAcquirableAbilityListReq>
        {
            public override void Write(IBuffer buffer, C2SSkillGetAcquirableAbilityListReq obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteByte(buffer, (byte) obj.Job);
            }

            public override C2SSkillGetAcquirableAbilityListReq Read(IBuffer buffer)
            {
                return new C2SSkillGetAcquirableAbilityListReq
                {
                    CharacterId = ReadUInt32(buffer),
                    Job = (JobId) ReadByte(buffer)
                };
            }
        }
    }
}
