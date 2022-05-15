using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillGetAcquirableAbilityListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_GET_ACQUIRABLE_ABILITY_LIST_REQ;

        public uint Unk0 { get; set; }
        public byte Job { get; set; }

        public class Serializer : PacketEntitySerializer<C2SSkillGetAcquirableAbilityListReq>
        {
            public override void Write(IBuffer buffer, C2SSkillGetAcquirableAbilityListReq obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteByte(buffer, obj.Job);
            }

            public override C2SSkillGetAcquirableAbilityListReq Read(IBuffer buffer)
            {
                return new C2SSkillGetAcquirableAbilityListReq
                {
                    Unk0 = ReadUInt32(buffer),
                    Job = ReadByte(buffer)
                };
            }
        }
    }
}