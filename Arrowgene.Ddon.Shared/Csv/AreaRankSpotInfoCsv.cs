using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class AreaRankSpotInfoCsv : CsvReaderWriter<AreaRankSpotInfo>
    {
        protected override int NumExpectedItems => 6;

        protected override AreaRankSpotInfo CreateInstance(string[] properties)
        {
            if (!uint.TryParse(properties[0], out uint areaId)) return null;
            if (!uint.TryParse(properties[1], out uint textIndex)) return null;
            if (!uint.TryParse(properties[2], out uint spotId)) return null;
            if (!uint.TryParse(properties[3], out uint unlockRank)) return null;
            if (!uint.TryParse(properties[4], out uint unlockQuest)) return null;
            if (!uint.TryParse(properties[5], out uint releaseOnly)) return null;

            var obj = new AreaRankSpotInfo()
            {
                AreaId = (QuestAreaId)areaId,
                TextIndex = textIndex,
                SpotId = spotId,
                UnlockRank = unlockRank,
                UnlockQuest = unlockQuest,
                ReleaseOnly = releaseOnly > 0
            };



            return obj;
        }
    }
}
