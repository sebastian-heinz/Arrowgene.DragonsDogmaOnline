using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class PawnCraftSkillCostRateCsv : CsvReaderWriter<PawnCraftSkillCostRate>
    {
        protected override int NumExpectedItems => 5;

        protected override PawnCraftSkillCostRate CreateInstance(string[] properties)
        {
            if (!uint.TryParse(properties[0], out uint total)) return null;
            if (!float.TryParse(properties[1], out float rate1)) return null;
            if (!float.TryParse(properties[2], out float rate2)) return null;
            if (!float.TryParse(properties[3], out float rate3)) return null;
            if (!float.TryParse(properties[4], out float rate4)) return null;

            var obj = new PawnCraftSkillCostRate()
            {
                Total = total,
                CostRate1 = rate1,
                CostRate2 = rate2,
                CostRate3 = rate3,
                CostRate4 = rate4,
            };

            return obj;
        }
    }
}
