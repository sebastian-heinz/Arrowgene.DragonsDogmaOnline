using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        private static readonly string[] AchievementProgressFields = new string[]
        {
            "character_id", "achievement_type", "achievement_param", "progress"
        };

        private readonly string SqlSelectAchievementProgress = $"SELECT {BuildQueryField(AchievementProgressFields)} FROM \"ddon_achievement_progress\" WHERE \"character_id\" = @character_id;";
        private readonly string SqlInsertAchievementProgress = $"INSERT INTO \"ddon_achievement_progress\" ({BuildQueryField(AchievementProgressFields)}) VALUES ({BuildQueryInsert(AchievementProgressFields)});";
        private readonly string SqlUpdateAchievementProgress = $"UPDATE \"ddon_achievement_progress\" SET {BuildQueryUpdate(AchievementProgressFields)} WHERE \"character_id\" = @character_id AND \"achievement_type\" = @achievement_type AND \"achievement_param\" = @achievement_param;";

        private static readonly string[] AchievementFields = new string[]
        {
            "character_id", "achievement_id", "date"
        };

        private readonly string SqlSelectAchievementStatus = $"SELECT {BuildQueryField(AchievementFields)} FROM \"ddon_achievement\" WHERE \"character_id\" = @character_id;";
        private readonly string SqlInsertAchievementStatus = $"INSERT INTO \"ddon_achievement\" ({BuildQueryField(AchievementFields)}) VALUES ({BuildQueryInsert(AchievementFields)});";

        private static readonly string[] AchievementUniqueCraftFields = new string[]
        {
            "character_id", "item_id", "craft_type"
        };

        private readonly string SqlSelectAchievementUniqueCraft = $"SELECT {BuildQueryField(AchievementUniqueCraftFields)} FROM \"ddon_achievement_unique_crafts\" WHERE \"character_id\" = @character_id;";
        private readonly string SqlInsertAchievementUniqueCraft = $"INSERT INTO \"ddon_achievement_unique_crafts\" ({BuildQueryField(AchievementUniqueCraftFields)}) VALUES ({BuildQueryInsert(AchievementProgressFields)}) ON CONFLICT DO NOTHING;";

        public Dictionary<(AchievementType, uint), uint> SelectAchievementProgress(uint characterId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                var progress = new Dictionary<(AchievementType, uint), uint>();
                ExecuteReader(connection,
                    SqlSelectAchievementProgress,
                    command => { AddParameter(command, "@character_id", characterId); },
                    reader =>
                    {
                        while (reader.Read())
                        {
                            var type = (AchievementType)GetUInt32(reader, "achievement_type");
                            var param = GetUInt32(reader, "achievement_param");
                            var prog = GetUInt32(reader, "progress");
                            progress[(type, param)] = prog;
                        }
                    });
                return progress;
            });
        }

        public bool UpsertAchievementProgress(uint characterId, AchievementType achievementType, uint achievementParam, uint progress, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                bool check = ExecuteNonQuery(
                    connection,
                    SqlUpdateAchievementProgress,
                    command =>
                    {
                        AddParameter(command, "character_id", characterId);
                        AddParameter(command, "achievement_type", (uint)achievementType);
                        AddParameter(command, "achievement_param", achievementParam);
                        AddParameter(command, "progress", progress);
                    }
                    ) == 1;

                if (!check)
                {
                    check = ExecuteNonQuery(
                    connection,
                    SqlInsertAchievementProgress,
                    command =>
                    {
                        AddParameter(command, "character_id", characterId);
                        AddParameter(command, "achievement_type", (uint)achievementType);
                        AddParameter(command, "achievement_param", achievementParam);
                        AddParameter(command, "progress", progress);
                    }
                    ) == 1;
                }

                return check;
            });
        }

        public Dictionary<uint, DateTimeOffset> SelectAchievementStatus(uint characterId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                var status = new Dictionary<uint, DateTimeOffset>();
                ExecuteReader(connection,
                    SqlSelectAchievementStatus,
                    command => { AddParameter(command, "@character_id", characterId); },
                    reader =>
                    {
                        while (reader.Read())
                        {
                            var achievementId = GetUInt32(reader, "achievement_id");
                            var date = GetDateTime(reader, "date");
                            status[achievementId] = date;
                        }
                    });
                return status;
            });
        }

        public bool InsertAchievementStatus(uint characterId, AchievementAsset achievement, bool reward = false, DbConnection? connectionIn = null)
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

        public Dictionary<AchievementCraftTypeParam, HashSet<ItemId>> SelectAchievementUniqueCrafts(uint characterId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                var uniqueCrafts = new Dictionary<AchievementCraftTypeParam, HashSet<ItemId>>();
                ExecuteReader(connection,
                    SqlSelectAchievementUniqueCraft,
                    command => { AddParameter(command, "@character_id", characterId); },
                    reader =>
                    {
                        while (reader.Read())
                        {
                            var craftType = (AchievementCraftTypeParam)GetUInt32(reader, "craft_type");
                            var itemId = (ItemId)GetUInt32(reader, "uint");

                            if (!uniqueCrafts.ContainsKey(craftType))
                            {
                                uniqueCrafts[craftType] = new();
                            }
                            uniqueCrafts[craftType].Add(itemId);
                        }
                    });
                return uniqueCrafts;
            });
        }

        public bool InsertAchievementUniqueCraft(uint characterId, AchievementCraftTypeParam craftType, ItemId itemId, DbConnection? connectionIn = null)
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
}
