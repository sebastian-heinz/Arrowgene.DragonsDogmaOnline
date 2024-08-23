using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.Shared.Model
{
    public class JobEmblem
    {
        public JobEmblem()
        {
            StatLevels = new();
            UIDs = new();
        }

        public JobId JobId { get; set; }
        public HashSet<string> UIDs { get; set; }
        public byte EmblemLevel { get; set; }
        public ushort EmblemPointsUsed { get; set; }

        public Dictionary<EquipStatId, byte> StatLevels { get; set; }

        public List<CDataJobEmblemStatParam> GetEmblemStatParamList()
        {
            var results = new List<CDataJobEmblemStatParam>()
            {
                new CDataJobEmblemStatParam() { StatId = EquipStatId.EmblemLevel, Add = EmblemLevel }
            };
            results.AddRange(StatLevels.Select(x => new CDataJobEmblemStatParam()
            {
                StatId = x.Key,
                Add = x.Value,
            }).ToList());
            return results;
        }
    }
}
