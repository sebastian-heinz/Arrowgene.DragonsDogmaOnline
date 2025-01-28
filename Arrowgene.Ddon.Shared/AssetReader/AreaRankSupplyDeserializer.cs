using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class AreaRankSupplyDeserializer : IAssetDeserializer<Dictionary<QuestAreaId, List<AreaRankSupply>>>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(AreaRankSupplyDeserializer));

        public Dictionary<QuestAreaId, List<AreaRankSupply>> ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            Dictionary<QuestAreaId, List<AreaRankSupply>> asset = new();

            string json = File.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            var areaRankSupplyElements = document.RootElement.EnumerateArray().ToList();
            foreach (var areaRankSupplyInfo in areaRankSupplyElements)
            {
                var sAreaId = areaRankSupplyInfo.GetProperty("AreaId").GetString();
                if (!Enum.TryParse(sAreaId, true, out QuestAreaId areaId))
                {
                    Logger.Error($"Invalid area id {sAreaId} in area supply asset.");
                    continue;
                };

                var minRank = areaRankSupplyInfo.GetProperty("MinRank").GetUInt32();
                var supplies = JsonSerializer.Deserialize<List<CDataBorderSupplyItem>>(areaRankSupplyInfo.GetProperty("SupplyItemInfoList"));

                if (!asset.ContainsKey(areaId))
                {
                    asset[areaId] = new();
                }

                asset[areaId].Add(new AreaRankSupply()
                {
                    AreaId = areaId,
                    MinRank = minRank,
                    SupplyItemInfoList = supplies
                });
            }

            return asset;
        }
    }
}
