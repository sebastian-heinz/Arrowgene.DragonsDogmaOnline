using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnSpSkillDeleteStockSkillRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_SP_SKILL_DELETE_STOCK_SKILL_RES;

        public class Serializer : PacketEntitySerializer<S2CPawnSpSkillDeleteStockSkillRes>
        {
            public override void Write(IBuffer buffer, S2CPawnSpSkillDeleteStockSkillRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CPawnSpSkillDeleteStockSkillRes Read(IBuffer buffer)
            {
                S2CPawnSpSkillDeleteStockSkillRes obj = new S2CPawnSpSkillDeleteStockSkillRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}