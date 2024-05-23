using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnSpSkillGetStockSkillReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_SP_SKILL_GET_STOCK_SKILL_REQ;

        public uint PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnSpSkillGetStockSkillReq>
        {
            public override void Write(IBuffer buffer, C2SPawnSpSkillGetStockSkillReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);                
            }

            public override C2SPawnSpSkillGetStockSkillReq Read(IBuffer buffer)
            {
                C2SPawnSpSkillGetStockSkillReq obj = new C2SPawnSpSkillGetStockSkillReq();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }

    }
}