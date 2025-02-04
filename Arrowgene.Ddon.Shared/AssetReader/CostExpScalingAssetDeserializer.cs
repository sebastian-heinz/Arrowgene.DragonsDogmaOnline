using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Asset;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class CostExpScalingAssetDeserializer : IAssetDeserializer<CostExpScalingAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(CostExpScalingAssetDeserializer));

        public CostExpScalingAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            CostExpScalingAsset asset = new CostExpScalingAsset();

            string json = Util.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            var costExpInfoElements = document.RootElement.EnumerateArray().ToList();
            foreach (var costExpInfo in costExpInfoElements)
            {
                var itemLevel = costExpInfo.GetProperty("item_level").GetUInt32();
                var cost = costExpInfo.GetProperty("cost").GetUInt32(); ;
                var exp = costExpInfo.GetProperty("exp").GetUInt32();

                asset.CostExpScalingInfo[itemLevel] = new CostExpInfo()
                {
                    ItemLevel = itemLevel,
                    Cost = cost,
                    Exp = exp
                };
            }

            return asset;
        }
    }
}

