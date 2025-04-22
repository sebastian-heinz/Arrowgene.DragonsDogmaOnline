using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
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

            string json = Util.ReadAllText(path);
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
                    eventItem.StageIds.Add(jStageId.GetUInt32());
                }

                foreach (var jEnemyId in jEventItem.GetProperty("enemy_ids").EnumerateArray())
                {
                    if (jEnemyId.ValueKind == JsonValueKind.String)
                    {
                        eventItem.EnemyIds.Add(Convert.ToUInt32(jEnemyId.GetString(), 16));
                    }
                    else
                    {
                        eventItem.EnemyIds.Add(jEnemyId.GetUInt32());
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
                                continue;
                            }
                        }
                    }

                    eventItem.EmClassConstraint = EventItemConstraint.None;
                    if (jRequirements.TryGetProperty("enemy_class", out JsonElement jEnemyClass))
                    {
                        if (!Enum.TryParse(jEnemyClass.ToString(), out eventItem.EmClassConstraint))
                        {
                            Logger.Error("Failed to parse the 'enemy_class' constraint.");
                            continue;
                        }

                        if (eventItem.EmClassConstraint != EventItemConstraint.None ||
                            eventItem.EmClassConstraint != EventItemConstraint.IsBoss ||
                            eventItem.EmClassConstraint != EventItemConstraint.IsNotBoss)
                        {
                            Logger.Error($"The constraint '{eventItem.EmClassConstraint}' is not a valid constraint for 'enemy_class'.");
                            continue;
                        }
                    }

                    eventItem.EmLvConstraint = EventItemConstraint.None;
                    if (jRequirements.TryGetProperty("enemy_level", out JsonElement jEnemyLevel))
                    {
                        if (!Enum.TryParse(jEnemyLevel.GetProperty("constraint").ToString(), out eventItem.EmLvConstraint))
                        {
                            Logger.Error("Required element 'constraint' does not exist or is an invalid value.");
                            continue;
                        }

                        switch (eventItem.EmLvConstraint)
                        {
                            case EventItemConstraint.LessThan:
                            case EventItemConstraint.LessThanOrEqual:
                            case EventItemConstraint.GreaterThan:
                            case EventItemConstraint.GreaterThanOrEqual:
                            case EventItemConstraint.InRange:
                                break;
                            default:
                                Logger.Error($"The constraint '{eventItem.EmLvConstraint}' is not a valid constraint for 'enemy_level'.");
                                continue;
                        }

                        uint Lv = 0;
                        if (jEnemyLevel.TryGetProperty("min_lv", out JsonElement jEmLv))
                        {
                            Lv = jEmLv.GetUInt32();
                        }

                        uint minLv = 0;
                        if (jEnemyLevel.TryGetProperty("min_lv", out JsonElement jEmMinLv))
                        {
                            minLv = jEmMinLv.GetUInt32();
                        }

                        uint maxLv = 0;
                        if (jEnemyLevel.TryGetProperty("max_lv", out JsonElement jEmMaxLv))
                        {
                            maxLv = jEmMaxLv.GetUInt32();
                        }

                        if ((eventItem.EmLvConstraint == EventItemConstraint.LessThan ||
                             eventItem.EmLvConstraint == EventItemConstraint.LessThanOrEqual ||
                             eventItem.EmLvConstraint == EventItemConstraint.GreaterThan ||
                             eventItem.EmLvConstraint == EventItemConstraint.GreaterThanOrEqual) && Lv == 0)
                        {
                            Logger.Error($"The constraint '{eventItem.EmLvConstraint}' requires the field 'lv' to be present with a value > 0.");
                            continue;
                        }
                        else if(eventItem.EmLvConstraint == EventItemConstraint.InRange && (minLv == 0 || maxLv == 0))
                        {
                            Logger.Error($"The constraint '{eventItem.EmLvConstraint}' requires the field 'min_lv' and 'max_lv' to be present with a value > 0.");
                            continue;
                        }

                        eventItem.EmLvConstraintParams = (Lv, minLv, maxLv);
                    }
                }

                asset.EventItems.Add(eventItem);
            }

            return asset;
        }
    }
}

