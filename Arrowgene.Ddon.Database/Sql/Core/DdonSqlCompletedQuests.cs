using Arrowgene.Ddon.Shared.Model.Quest;
using System.Collections.Generic;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        /* ddon_completed_quests */
        protected static readonly string[] CompletedQuestsFields = new string[]
        {
            "character_common_id", "quest_type", "quest_id", "clear_count"
        };

        private readonly string SqlInsertCompletedQuestId = $"INSERT INTO \"ddon_completed_quests\" ({BuildQueryField(CompletedQuestsFields)}) VALUES ({BuildQueryInsert(CompletedQuestsFields)});";
        private readonly string SqlInsertIfNotExistCompletedQuestId = $"INSERT INTO \"ddon_completed_quests\" ({BuildQueryField(CompletedQuestsFields)}) SELECT " +
                                                                         $"{BuildQueryInsert(CompletedQuestsFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_completed_quests\" WHERE " +
                                                                         $"\"character_common_id\" = @character_common_id AND \"quest_id\" = @quest_id);";
        private readonly string SqlSelectCompletedQuestByType = $"SELECT {BuildQueryField(CompletedQuestsFields)} FROM \"ddon_completed_quests\" WHERE \"character_common_id\" = @character_common_id AND \"quest_type\" = @quest_type;";
        private readonly string SqlSelectCompletedQuestById = $"SELECT {BuildQueryField(CompletedQuestsFields)} FROM \"ddon_completed_quests\" WHERE \"character_common_id\" = @character_common_id AND \"quest_id\" = @quest_id;";

        public List<CompletedQuest> GetCompletedQuestsByType(uint characterCommonId, QuestType questType)
        {
            using TCon connection = OpenNewConnection();
            return GetCompletedQuestsByType(connection, characterCommonId, questType);
        }

        public List<CompletedQuest> GetCompletedQuestsByType(TCon connection, uint characterCommonId, QuestType questType)
        {
            List<CompletedQuest> results = new List<CompletedQuest>();

            ExecuteInTransaction(conn =>
            {
                ExecuteReader(conn, SqlSelectCompletedQuestByType,
                    command => {
                        AddParameter(command, "@character_common_id", characterCommonId);
                        AddParameter(command, "@quest_type", (uint)questType);
                    }, reader => {
                        while (reader.Read())
                        {

                            results.Add(new CompletedQuest()
                            {
                                QuestId = (QuestId) GetUInt32(reader, "quest_id"),
                                QuestType = questType,
                                ClearCount = GetUInt32(reader, "clear_count")
                            });
                        }
                    });
            });

            return results;
        }

        public CompletedQuest GetCompletedQuestsById(uint characterCommonId, QuestId questId)
        {
            using TCon connection = OpenNewConnection();
            return GetCompletedQuestsById(connection, characterCommonId, questId);
        }

        public CompletedQuest GetCompletedQuestsById(TCon connection, uint characterCommonId, QuestId questId)
        {
            CompletedQuest result = null;

            ExecuteInTransaction(conn =>
            {
                ExecuteReader(conn, SqlSelectCompletedQuestById,
                    command => {
                        AddParameter(command, "@character_common_id", characterCommonId);
                        AddParameter(command, "@quest_id", (uint) questId);
                    }, reader => {
                        if (reader.Read())
                        {
                            result = new CompletedQuest()
                            {
                                QuestId = questId,
                                QuestType = (QuestType) GetUInt32(reader, "quest_type"),
                                ClearCount = GetUInt32(reader, "clear_count")
                            };
                        }
                    });
            });

            return result;
        }

        public bool InsertIfNotExistCompletedQuest(uint characterCommonId, QuestId questId, QuestType questType)
        {
            using TCon connection = OpenNewConnection();
            return InsertIfNotExistCompletedQuest(connection, characterCommonId, questId, questType);
        }

        public bool InsertIfNotExistCompletedQuest(TCon connection, uint characterCommonId, QuestId questId, QuestType questType)
        {
            return ExecuteNonQuery(connection, SqlInsertIfNotExistCompletedQuestId, command =>
            {
                AddParameter(command, "character_common_id", characterCommonId);
                AddParameter(command, "quest_id", (uint) questId);
                AddParameter(command, "quest_type", (uint) questType);
                AddParameter(command, "clear_count", 1);
            }) == 1;
        }
    }
}

