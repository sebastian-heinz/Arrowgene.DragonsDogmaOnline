using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Asset;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class ElementAttachInfoAssetDeserializer : IAssetDeserializer<ElementAttachInfoAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(ElementAttachInfoAssetDeserializer));

        public ElementAttachInfoAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            ElementAttachInfoAsset asset = new ElementAttachInfoAsset();

            string json = File.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            var elementAttachInfoElements = document.RootElement.EnumerateArray().ToList();
            foreach (var elementAttachInfo in elementAttachInfoElements)
            {
                var itemLevel = elementAttachInfo.GetProperty("item_level").GetUInt32();
                var cost = elementAttachInfo.GetProperty("cost").GetUInt32(); ;
                var exp = elementAttachInfo.GetProperty("exp").GetUInt32();

                asset.ElementAttachInfo[itemLevel] = new ElementAttachInfo()
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

