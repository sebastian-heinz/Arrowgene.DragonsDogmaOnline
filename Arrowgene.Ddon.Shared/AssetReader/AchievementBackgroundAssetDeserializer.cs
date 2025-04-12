using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class AchievementBackgroundAssetDeserializer : IAssetDeserializer<AchievementBackgroundAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(AchievementBackgroundAssetDeserializer));

        public AchievementBackgroundAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            AchievementBackgroundAsset asset = new();

            string json = Util.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            asset.DefaultBackgrounds = document.RootElement.GetProperty("background_data").GetProperty("default_backgrounds").EnumerateArray().Select(x => x.GetUInt32()).ToHashSet();
            asset.UnlockableBackgrounds = document.RootElement.GetProperty("background_data").GetProperty("unlockable_backgrounds").EnumerateArray().Select(x => (
                        x.GetProperty("id").GetUInt32(),
                        x.GetProperty("required_achievements").GetUInt32()
                    )).ToHashSet();

            return asset;
        }
    }
}
