using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPawnSpSkillDeleteStockSkillReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PAWN_SP_SKILL_DELETE_STOCK_SKILL_REQ;

        public uint PawnId { get; set; }
        public byte SpSkillId { get; set; }
        public byte SpSkillLv { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPawnSpSkillDeleteStockSkillReq>
        {
            public override void Write(IBuffer buffer, C2SPawnSpSkillDeleteStockSkillReq obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, obj.SpSkillId);
                WriteByte(buffer, obj.SpSkillLv);
            }

            public override C2SPawnSpSkillDeleteStockSkillReq Read(IBuffer buffer)
            {
                C2SPawnSpSkillDeleteStockSkillReq obj = new C2SPawnSpSkillDeleteStockSkillReq();
                obj.PawnId = ReadUInt32(buffer);
                obj.SpSkillId = ReadByte(buffer);
                obj.SpSkillLv = ReadByte(buffer);
                return obj;
            }
        }

    }
}