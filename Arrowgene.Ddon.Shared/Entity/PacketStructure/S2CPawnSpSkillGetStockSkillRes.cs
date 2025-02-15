using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnSpSkillGetStockSkillRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_PAWN_SP_SKILL_GET_STOCK_SKILL_RES;

        public S2CPawnSpSkillGetStockSkillRes()
        {
            SpSkillList = new List<CDataSpSkill>();
        }

        public List<CDataSpSkill> SpSkillList { get; set; }
        public uint StockSlots { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnSpSkillGetStockSkillRes>
        {
            public override void Write(IBuffer buffer, S2CPawnSpSkillGetStockSkillRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataSpSkill>(buffer, obj.SpSkillList);
                WriteUInt32(buffer, obj.StockSlots);
            }

            public override S2CPawnSpSkillGetStockSkillRes Read(IBuffer buffer)
            {
                S2CPawnSpSkillGetStockSkillRes obj = new S2CPawnSpSkillGetStockSkillRes();
                ReadServerResponse(buffer, obj);
                obj.SpSkillList = ReadEntityList<CDataSpSkill>(buffer);
                obj.StockSlots = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}