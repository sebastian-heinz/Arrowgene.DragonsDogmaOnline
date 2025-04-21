using System;
using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private static readonly string[] AchievementProgressFields =
    [
        "character_id", "achievement_type", "achievement_param", "progress"
    ];

    private static readonly string[] AchievementFields =
    [
        "character_id", "achievement_id", "date"
    ];

    private static readonly string[] AchievementUniqueCraftFields =
    [
        "character_id", "item_id", "craft_type"
    ];

    private readonly string SqlInsertAchievementProgress =
        $"INSERT INTO \"ddon_achievement_progress\" ({BuildQueryField(AchievementProgressFields)}) VALUES ({BuildQueryInsert(AchievementProgressFields)});";

    private readonly string SqlInsertAchievementStatus =
        $"INSERT INTO \"ddon_achievement\" ({BuildQueryField(AchievementFields)}) VALUES ({BuildQueryInsert(AchievementFields)});";

    private readonly string SqlInsertAchievementUniqueCraft =
        $"INSERT INTO \"ddon_achievement_unique_crafts\" ({BuildQueryField(AchievementUniqueCraftFields)}) VALUES ({BuildQueryInsert(AchievementUniqueCraftFields)}) ON CONFLICT DO NOTHING;";

    private readonly string SqlSelectAchievementProgress =
        $"SELECT {BuildQueryField(AchievementProgressFields)} FROM \"ddon_achievement_progress\" WHERE \"character_id\" = @character_id;";

    private readonly string SqlSelectAchievementStatus = $"SELECT {BuildQueryField(AchievementFields)} FROM \"ddon_achievement\" WHERE \"character_id\" = @character_id;";

    private readonly string SqlSelectAchievementUniqueCraft =
        $"SELECT {BuildQueryField(AchievementUniqueCraftFields)} FROM \"ddon_achievement_unique_crafts\" WHERE \"character_id\" = @character_id;";

    private readonly string SqlUpdateAchievementProgress =
        $"UPDATE \"ddon_achievement_progress\" SET {BuildQueryUpdate(AchievementProgressFields)} WHERE \"character_id\" = @character_id AND \"achievement_type\" = @achievement_type AND \"achievement_param\" = @achievement_param;";

    private readonly string SqlUpsertAchievementProgress =
        $"INSERT INTO \"ddon_achievement_progress\" ({BuildQueryField(AchievementProgressFields)}) " +
        $"VALUES ({BuildQueryInsert(AchievementProgressFields)}) " +
        "ON CONFLICT (\"character_id\", \"achievement_type\", \"achievement_param\") " +
        "DO UPDATE SET \"progress\" = EXCLUDED.\"progress\";";
    
    public override Dictionary<(AchievementType, uint), uint> SelectAchievementProgress(uint characterId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            Dictionary<(AchievementType, uint), uint> progress = new();
            ExecuteReader(connection,
                SqlSelectAchievementProgress,
                command => { AddParameter(command, "@character_id", characterId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        AchievementType type = (AchievementType)GetUInt32(reader, "achievement_type");
                        uint param = GetUInt32(reader, "achievement_param");
                        uint prog = GetUInt32(reader, "progress");
                        progress[(type, param)] = prog;
                    }
                });
            return progress;
        });
    }

    public override bool UpsertAchievementProgress(uint characterId, AchievementType achievementType, uint achievementParam, uint progress, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            int affected = ExecuteNonQuery(
                connection,
                SqlUpsertAchievementProgress,
                command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "achievement_type", (uint)achievementType);
                    AddParameter(command, "achievement_param", achievementParam);
                    AddParameter(command, "progress", progress);
                }
            );
            return affected > 0;
        });
    }
    
    public override Dictionary<uint, DateTimeOffset> SelectAchievementStatus(uint characterId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            Dictionary<uint, DateTimeOffset> status = new();
            ExecuteReader(connection,
                SqlSelectAchievementStatus,
                command => { AddParameter(command, "@character_id", characterId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        uint achievementId = GetUInt32(reader, "achievement_id");
                        DateTime date = GetDateTime(reader, "date");
                        status[achievementId] = date;
                    }
                });
            return status;
        });
    }

    public override bool InsertAchievementStatus(uint characterId, AchievementAsset achievement, bool reward = false, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(
                connection,
                SqlInsertAchievementStatus,
                command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "achievement_id", achievement.Id);
                    AddParameter(command, "date", DateTime.UtcNow);
                }
            ) == 1;
        });
    }

    public override Dictionary<AchievementCraftTypeParam, HashSet<ItemId>> SelectAchievementUniqueCrafts(uint characterId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            Dictionary<AchievementCraftTypeParam, HashSet<ItemId>> uniqueCrafts = new();
            ExecuteReader(connection,
                SqlSelectAchievementUniqueCraft,
                command => { AddParameter(command, "@character_id", characterId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        AchievementCraftTypeParam craftType = (AchievementCraftTypeParam)GetUInt32(reader, "craft_type");
                        ItemId itemId = (ItemId)GetUInt32(reader, "item_id");

                        if (!uniqueCrafts.ContainsKey(craftType)) uniqueCrafts[craftType] = new HashSet<ItemId>();

                        uniqueCrafts[craftType].Add(itemId);
                    }
                });
            return uniqueCrafts;
        });
    }

    public override bool InsertAchievementUniqueCraft(uint characterId, AchievementCraftTypeParam craftType, ItemId itemId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(
                connection,
                SqlInsertAchievementUniqueCraft,
                command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "craft_type", (uint)craftType);
                    AddParameter(command, "item_id", (uint)itemId);
                }
            ) == 1;
        });
    }
}
