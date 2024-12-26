using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Logging;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class RecruitmentBoardCategoryDeserializer : IAssetDeserializer<RecruitmentBoardCategoryAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(SecretAbilityDeserializer));

        public RecruitmentBoardCategoryAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            RecruitmentBoardCategoryAsset asset = new RecruitmentBoardCategoryAsset();

            string json = Util.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            foreach (var category in document.RootElement.EnumerateArray().ToList())
            {
                var data = new BoardCategory()
                {
                    CategoryId = category.GetProperty("id").GetUInt32(),
                    CategoryName = category.GetProperty("name").GetString()
                };

                data.PartyMin = 2;
                if (category.TryGetProperty("party_min", out JsonElement jPartyMin))
                {
                    data.PartyMin = jPartyMin.GetUInt16();
                }

                data.PartyMax = 4;
                if (category.TryGetProperty("party_min", out JsonElement jPartyMax))
                {
                    data.PartyMax = jPartyMax.GetUInt16();
                }

                asset.RecruitmentBoardCategories[data.CategoryId] = data;
            }

            return asset;
        }
    }
}

