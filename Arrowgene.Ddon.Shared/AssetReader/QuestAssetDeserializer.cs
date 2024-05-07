using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Text.Json.Serialization;

namespace Arrowgene.Ddon.Shared.Csv
{
    public class QuestAssetDeserializer : IAssetDeserializer<QuestAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(QuestAssetDeserializer));

        public QuestAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            QuestAsset asset = new QuestAsset();

            string json = File.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            List<CDataQuestList> missionData = JsonSerializer.Deserialize<List<CDataQuestList>>(document.RootElement.GetProperty("missions"));
            foreach (var mission in missionData)
            {
                asset.MainQuests[mission.QuestId] = mission;
            }

            return asset;
        }
    }
}
