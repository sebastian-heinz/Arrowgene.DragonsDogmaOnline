using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnSpSkillSetActiveSkillRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_SP_SKILL_SET_ACTIVE_SKILL_RES;

        public class Serializer : PacketEntitySerializer<S2CPawnSpSkillSetActiveSkillRes>
        {
            public override void Write(IBuffer buffer, S2CPawnSpSkillSetActiveSkillRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPawnSpSkillSetActiveSkillRes Read(IBuffer buffer)
            {
                S2CPawnSpSkillSetActiveSkillRes obj = new S2CPawnSpSkillSetActiveSkillRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}