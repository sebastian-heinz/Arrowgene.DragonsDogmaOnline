using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Asset;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class SecretAbilityDeserializer : IAssetDeserializer<SecretAbilityAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(SecretAbilityDeserializer));

        public SecretAbilityAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            SecretAbilityAsset asset = new SecretAbilityAsset();

            string json = Util.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            var secretAbilities = document.RootElement.GetProperty("default_abilitiles").EnumerateArray().ToList();
            foreach (var abilityId in secretAbilities)
            {
                asset.DefaultSecretAbilities.Add((AbilityId) abilityId.GetUInt32());
            }

            return asset;
        }
    }
}
