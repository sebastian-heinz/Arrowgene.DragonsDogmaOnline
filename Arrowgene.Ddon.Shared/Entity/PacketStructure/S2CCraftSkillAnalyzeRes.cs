using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraftSkillAnalyzeRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CRAFT_CRAFT_SKILL_ANALYZE_RES;

        public S2CCraftSkillAnalyzeRes()
        {
            AnalyzeResultList = new List<CDataCraftSkillAnalyzeResult>();
        }

        public List<CDataCraftSkillAnalyzeResult> AnalyzeResultList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CCraftSkillAnalyzeRes>
        {
            public override void Write(IBuffer buffer, S2CCraftSkillAnalyzeRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.AnalyzeResultList);
            }

            public override S2CCraftSkillAnalyzeRes Read(IBuffer buffer)
            {
                S2CCraftSkillAnalyzeRes obj = new S2CCraftSkillAnalyzeRes();
                ReadServerResponse(buffer, obj);
                obj.AnalyzeResultList = ReadEntityList<CDataCraftSkillAnalyzeResult>(buffer);
                return obj;
            }
        }
    }
}
