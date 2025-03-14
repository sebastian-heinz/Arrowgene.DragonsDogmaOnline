using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class PawnCraftSkillSpeedRateCsv : CsvReaderWriter<PawnCraftSkillSpeedRate>
    {
        protected override int NumExpectedItems => 3;

        protected override PawnCraftSkillSpeedRate CreateInstance(string[] properties)
        {
            if (!uint.TryParse(properties[0], out uint level)) return null;
            if (!float.TryParse(properties[1], out float rate1)) return null;
            if (!float.TryParse(properties[2], out float rate2)) return null;

            var obj = new PawnCraftSkillSpeedRate()
            {
                Level = level,
                SpeedRate1 = rate1,
                SpeedRate2 = rate2,
            };

            return obj;
        }
    }
}
