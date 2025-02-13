using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class AreaRankRequirementDeserializer : IAssetDeserializer<Dictionary<QuestAreaId, List<AreaRankRequirement>>>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(AreaRankRequirementDeserializer));

        public Dictionary<QuestAreaId, List<AreaRankRequirement>> ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            Dictionary<QuestAreaId, List<AreaRankRequirement>> asset = new();

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

                asset.Add(areaId, new());

                var reqElements = areaElement.GetProperty("RankRequirements").EnumerateArray();
                foreach (var reqElement in reqElements)
                {
                    uint rank = reqElement.GetProperty("Rank").GetUInt32();
                    uint minPoint = reqElement.TryGetProperty("MinPoint", out JsonElement jMinPoint) ? jMinPoint.GetUInt32() : 0;
                    uint areaTrial = reqElement.TryGetProperty("AreaTrial", out JsonElement jAreaTrial) ? jAreaTrial.GetUInt32() : 0;
                    uint extQuest = reqElement.TryGetProperty("ExtQuest", out JsonElement jExtQuest) ? jExtQuest.GetUInt32() : 0;

                    asset[areaId].Add(new AreaRankRequirement()
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
