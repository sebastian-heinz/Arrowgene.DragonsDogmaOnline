using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class AreaRankRequirementDeserializer : IAssetDeserializer<List<AreaRankRequirement>>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(AreaRankRequirementDeserializer));

        public List<AreaRankRequirement> ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            List<AreaRankRequirement> asset = new();

            string json = File.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            var areaElements = document.RootElement.EnumerateArray();
            foreach (var areaElement in areaElements)
            {
                var sAreaId = areaElement.GetProperty("AreaId").GetString();
                if (!Enum.TryParse(sAreaId, true, out QuestAreaId areaId))
                {
                    Logger.Error($"Invalid area id {sAreaId} in area requirement asset.");
                    continue;
                };

                var reqElements = areaElement.GetProperty("RankRequirements").EnumerateArray();
                foreach (var reqElement in reqElements)
                {
                    uint rank = reqElement.GetProperty("Rank").GetUInt32();
                    uint minPoint = reqElement.TryGetProperty("MinPoint", out JsonElement jMinPoint) ? jMinPoint.GetUInt32() : 0;
                    uint areaTrial = reqElement.TryGetProperty("AreaTrial", out JsonElement jAreaTrial) ? jAreaTrial.GetUInt32() : 0;
                    uint extQuest = reqElement.TryGetProperty("ExtQuest", out JsonElement jExtQuest) ? jExtQuest.GetUInt32() : 0;

                    asset.Add(new AreaRankRequirement()
                    {
                        AreaId = areaId,
                        Rank = rank,
                        MinPoint = minPoint,
                        AreaTrial = areaTrial,
                        ExternalQuest = extQuest,
                    });
                }
            }

            return asset;
        }
    }
}
