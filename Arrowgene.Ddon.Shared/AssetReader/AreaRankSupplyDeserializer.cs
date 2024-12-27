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
    public class AreaRankSupplyDeserializer : IAssetDeserializer<List<AreaRankSupply>>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(AreaRankSupplyDeserializer));

        public List<AreaRankSupply> ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            List<AreaRankSupply> asset = new();

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

                asset.Add(new AreaRankSupply()
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
