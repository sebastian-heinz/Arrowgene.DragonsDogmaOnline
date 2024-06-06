using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Web;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;

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
            "character_common_id", "quest_type", "quest_id"
        };

        private readonly string SqlInsertCompletedQuestId = $"INSERT INTO \"ddon_completed_quests\" ({BuildQueryField(CompletedQuestsFields)}) VALUES ({BuildQueryInsert(CompletedQuestsFields)});";
        private readonly string SqlInsertIfNotExistCompletedQuestId = $"INSERT INTO \"ddon_completed_quests\" ({BuildQueryField(CompletedQuestsFields)}) SELECT " +
                                                                         $"{BuildQueryInsert(CompletedQuestsFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_completed_quests\" WHERE " +
                                                                         $"\"character_common_id\" = @character_common_id AND \"quest_id\" = @quest_id);";
        private readonly string SqlSelectCompletedQuestByType = $"SELECT {BuildQueryField(CompletedQuestsFields)} FROM \"ddon_completed_quests\" WHERE \"character_common_id\" = @character_common_id AND \"quest_type\" = @quest_type;";

        public List<QuestId> GetCompletedQuestsByType(uint characterCommonId, QuestType questType)
        {
            using TCon connection = OpenNewConnection();
            return GetCompletedQuestsByType(connection, characterCommonId, questType);
        }

        public List<QuestId> GetCompletedQuestsByType(TCon connection, uint characterCommonId, QuestType questType)
        {
            List<QuestId> results = new List<QuestId>();

            ExecuteInTransaction(conn =>
            {
                ExecuteReader(conn, SqlSelectCompletedQuestByType,
                    command => {
                        AddParameter(command, "@character_common_id", characterCommonId);
                        AddParameter(command, "@quest_type", (uint)questType);
                    }, reader => {
                        while (reader.Read())
                        {
                            results.Add((QuestId)GetUInt32(reader, "quest_id"));
                        }
                    });
            });

            return results;
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
            }) == 1;
        }
    }
}

