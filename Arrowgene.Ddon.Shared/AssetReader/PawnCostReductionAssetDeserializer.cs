using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Asset;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class PawnCostReductionAssetDeserializer : IAssetDeserializer<PawnCostReductionAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(PawnCostReductionAssetDeserializer));

        public PawnCostReductionAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            var asset = new PawnCostReductionAsset();

            string json = Util.ReadAllText(path);
            using (JsonDocument document = JsonDocument.Parse(json))
            {
                var costReductionElements = document.RootElement.EnumerateArray().ToList();
                foreach (var element in costReductionElements)
                {
                    uint total = element.GetProperty("Total").GetUInt32();
                    float costRate1 = element.GetProperty("CostRate1").GetSingle();
                    float costRate2 = element.GetProperty("CostRate2").GetSingle();
                    float costRate3 = element.GetProperty("CostRate3").GetSingle();
                    float costRate4 = element.GetProperty("CostRate4").GetSingle();

                    // In this example, `Total` is used as the key for the dictionary
                    asset.PawnCostReductionInfo[total] = new PawnCostReductionInfo()
                    {
                        Total = total,
                        CostRate1 = costRate1,
                        CostRate2 = costRate2,
                        CostRate3 = costRate3,
                        CostRate4 = costRate4
                    };
                }
            }

            return asset;
        }
    }
}
