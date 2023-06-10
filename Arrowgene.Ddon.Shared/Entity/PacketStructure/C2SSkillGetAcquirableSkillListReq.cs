using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SSkillGetAcquirableSkillListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SKILL_GET_ACQUIRABLE_SKILL_LIST_REQ;

        public C2SSkillGetAcquirableSkillListReq()
        {
            CharacterId = 0;
            Job = 0;
        }

        public uint CharacterId { get; set; }
        public JobId Job { get; set; }


        public class Serializer : PacketEntitySerializer<C2SSkillGetAcquirableSkillListReq>
        {
            public override void Write(IBuffer buffer, C2SSkillGetAcquirableSkillListReq obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteByte(buffer, (byte) obj.Job);
            }

            public override C2SSkillGetAcquirableSkillListReq Read(IBuffer buffer)
            {
                return new C2SSkillGetAcquirableSkillListReq
                {
                    CharacterId = ReadUInt32(buffer),
                    Job = (JobId) ReadByte(buffer)
                };
            }
        }
    }
}