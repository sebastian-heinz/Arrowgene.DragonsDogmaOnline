using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnSpSkillGetActiveSkillRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_SP_SKILL_GET_ACTIVE_SKILL_RES;

        public S2CPawnSpSkillGetActiveSkillRes()
        {
            SpSkillList = new List<CDataSpSkill>();
        }

        public List<CDataSpSkill> SpSkillList { get; set; }
        public uint ActiveSpSkillSlots { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnSpSkillGetActiveSkillRes>
        {
            public override void Write(IBuffer buffer, S2CPawnSpSkillGetActiveSkillRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataSpSkill>(buffer, obj.SpSkillList);
                WriteUInt32(buffer, obj.ActiveSpSkillSlots);
            }

            public override S2CPawnSpSkillGetActiveSkillRes Read(IBuffer buffer)
            {
                S2CPawnSpSkillGetActiveSkillRes obj = new S2CPawnSpSkillGetActiveSkillRes();
                ReadServerResponse(buffer, obj);
                obj.SpSkillList = ReadEntityList<CDataSpSkill>(buffer);
                obj.ActiveSpSkillSlots = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}