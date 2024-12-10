using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Arrowgene.Ddon.Shared.AssetReader
{
    public class EventDropAssetDeserializer : IAssetDeserializer<EventDropsAsset>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(EventDropAssetDeserializer));

        public EventDropsAsset ReadPath(string path)
        {
            Logger.Info($"Reading {path}");

            EventDropsAsset asset = new EventDropsAsset();

            string json = File.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);

            foreach (var jEventItem in document.RootElement.EnumerateArray())
            {
                var eventItem = new EventItem()
                {
                    ItemId = jEventItem.GetProperty("item_id").GetUInt32(),
                    MinAmount = jEventItem.GetProperty("min_num").GetUInt32(),
                    MaxAmount = jEventItem.GetProperty("max_num").GetUInt32(),
                    Chance = jEventItem.GetProperty("drop_chance").GetDouble(),
                };

                foreach (var jQuestId in jEventItem.GetProperty("quest_ids").EnumerateArray())
                {
                    eventItem.QuestIds.Add(jQuestId.GetUInt32());
                }

                foreach (var jStageId in jEventItem.GetProperty("stage_ids").EnumerateArray())
                {
                    eventItem.QuestIds.Add(jStageId.GetUInt32());
                }

                foreach (var jEnemyId in jEventItem.GetProperty("enemy_ids").EnumerateArray())
                {
                    if (jEnemyId.ValueKind == JsonValueKind.String)
                    {
                        eventItem.QuestIds.Add(Convert.ToUInt32(jEnemyId.GetString(), 16));
                    }
                    else
                    {
                        eventItem.QuestIds.Add(jEnemyId.GetUInt32());
                    }
                }

                if (jEventItem.TryGetProperty("requirements", out JsonElement jRequirements))
                {
                    eventItem.RequiresLanternLit = false;
                    if (jRequirements.TryGetProperty("lantern_on", out JsonElement jLanternOn))
                    {
                        eventItem.RequiresLanternLit = jLanternOn.GetBoolean();
                    }

                    if (jRequirements.TryGetProperty("items_equipped", out JsonElement jItemsEquipped))
                    {
                        foreach (var jEquippedItem in jItemsEquipped.GetProperty("item_ids").EnumerateArray())
                        {
                            eventItem.RequiredItemsEquipped.Add(jEquippedItem.GetUInt32());
                        }

                        eventItem.ItemConstraint = EventItemConstraint.All;
                        if (jItemsEquipped.TryGetProperty("constraint", out JsonElement jConstraint))
                        {
                            if (Enum.TryParse(jConstraint.GetString(), true, out EventItemConstraint itemConstraint))
                            {
                                eventItem.ItemConstraint = itemConstraint;
                            }
                            else
                            {
                                Logger.Error($"Failed to parse item constraint {jConstraint.GetString()}");
                            }
                        }
                    }
                }

                asset.EventItems.Add(eventItem);
            }

            return asset;
        }
    }
}

