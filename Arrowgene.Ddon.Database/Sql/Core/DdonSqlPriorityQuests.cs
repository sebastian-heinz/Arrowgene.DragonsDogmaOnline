using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
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
        protected static readonly string[] PriorityQuestFields = new string[]
        {
            "character_common_id", "quest_schedule_id"
        };

        private readonly string SqlInsertIfNotExistPriorityQuestId = $"INSERT INTO \"ddon_priority_quests\" ({BuildQueryField(PriorityQuestFields)}) SELECT " +
                                                                         $"{BuildQueryInsert(PriorityQuestFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_priority_quests\" WHERE " +
                                                                         $"\"character_common_id\" = @character_common_id AND \"quest_schedule_id\" = @quest_schedule_id);";
        private readonly string SqlSelectPriorityQuests = $"SELECT {BuildQueryField(PriorityQuestFields)} FROM \"ddon_priority_quests\" WHERE \"character_common_id\" = @character_common_id;";
        private readonly string SqlDeletePriorityQuest = $"DELETE FROM \"ddon_priority_quests\" WHERE \"character_common_id\" = @character_common_id AND \"quest_schedule_id\" = @quest_schedule_id;";

        public bool InsertPriorityQuest(uint characterCommonId, uint questScheduleId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe<bool>(connectionIn, (connection) =>
            {
                return ExecuteNonQuery(connection, SqlInsertIfNotExistPriorityQuestId, command =>
                {
                    AddParameter(command, "character_common_id", characterCommonId);
                    AddParameter(command, "quest_schedule_id", questScheduleId);
                }) == 1;
            });
        }

        public List<uint> GetPriorityQuestScheduleIds(uint characterCommonId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe<List<uint>>(connectionIn, (connection) =>
            {
                List<uint> results = new List<uint>();
                ExecuteReader(connection, SqlSelectPriorityQuests,
                    command => {
                        AddParameter(command, "@character_common_id", characterCommonId);
                    }, reader => {
                        while (reader.Read())
                        {
                            results.Add(GetUInt32(reader, "quest_schedule_id"));
                        }
                    });
                return results;
            });
        }

        public bool DeletePriorityQuest(uint characterCommonId, uint questScheduleId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe<bool>(connectionIn, (connection) =>
            {
                return ExecuteNonQuery(connection, SqlDeletePriorityQuest, command =>
                {
                    AddParameter(command, "@character_common_id", characterCommonId);
                    AddParameter(command, "quest_schedule_id", questScheduleId);
                }) == 1;
            });
        }
    }
}

