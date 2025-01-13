using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class ClanShopCsv : CsvReaderWriter<ClanShopAsset>
    {
        protected override int NumExpectedItems => 9;

        protected override ClanShopAsset CreateInstance(string[] properties)
        {
            if (!uint.TryParse(properties[0], out uint lineupId)) return null;
            string name = properties[1];
            if (!uint.TryParse(properties[2], out uint cost)) return null;
            if (!byte.TryParse(properties[3], out byte level)) return null;
            if (!uint.TryParse(properties[4], out uint type)) return null;
            if (!uint.TryParse(properties[5], out uint icon)) return null;
            if (!uint.TryParse(properties[6], out uint requires)) return null;
            if (!uint.TryParse(properties[7], out uint subid)) return null;
            if (!byte.TryParse(properties[8], out byte subtype)) return null;

            var obj = new ClanShopAsset()
            {
                LineupId = lineupId,
                Name = name,
                RequireClanPoint = cost,
                RequireLevel = level,
                Type = (ClanShopLineupType)type,
                IconID = icon,
                Require = requires,
                SubId = subid,
                SubType = subtype
            };

            return obj;
        }
    }
}
