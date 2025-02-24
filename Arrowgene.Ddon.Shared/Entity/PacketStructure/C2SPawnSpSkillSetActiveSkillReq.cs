using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnSpSkillSetActiveSkillReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_SP_SKILL_SET_ACTIVE_SKILL_REQ;

        public uint PawnId { get; set; }
        public JobId JobId { get; set; }
        public CDataSpSkill FromStockSpSkill { get; set; } = new();
        public CDataSpSkill ToActiveSpSkill { get; set; } = new();

        public class Serializer : PacketEntitySerializer<C2SPawnSpSkillSetActiveSkillReq>
        {
            public override void Write(IBuffer buffer, C2SPawnSpSkillSetActiveSkillReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, (byte) obj.JobId);
                WriteEntity<CDataSpSkill>(buffer, obj.FromStockSpSkill);
                WriteEntity<CDataSpSkill>(buffer, obj.ToActiveSpSkill);
            }

            public override C2SPawnSpSkillSetActiveSkillReq Read(IBuffer buffer)
            {
                C2SPawnSpSkillSetActiveSkillReq obj = new C2SPawnSpSkillSetActiveSkillReq();
                obj.PawnId = ReadUInt32(buffer);
                obj.JobId = (JobId) ReadByte(buffer);
                obj.FromStockSpSkill = ReadEntity<CDataSpSkill>(buffer);
                obj.ToActiveSpSkill = ReadEntity<CDataSpSkill>(buffer);
                return obj;
            }
        }

    }
}
