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
        /* ddon_quest_progress */
        protected static readonly string[] QuestProgressFields = new string[]
        {
            "character_common_id", "quest_type", "quest_schedule_id", "step"
        };

        private readonly string SqlInsertQuestProgress = $"INSERT INTO \"ddon_quest_progress\" ({BuildQueryField(QuestProgressFields)}) VALUES ({BuildQueryInsert(QuestProgressFields)});";
        private readonly string SqlDeleteQuestProgress = $"DELETE FROM \"ddon_quest_progress\" WHERE \"character_common_id\"=@character_common_id AND \"quest_type\"=@quest_type AND \"quest_schedule_id\"=@quest_schedule_id;";

        private readonly string SqlUpdateQuestProgress = $"UPDATE \"ddon_quest_progress\" SET \"step\"=@step WHERE \"character_common_id\"=@character_common_id AND \"quest_type\"=@quest_type AND \"quest_schedule_id\"=@quest_schedule_id;";

        private readonly string SqlSelectQuestProgressByType = $"SELECT {BuildQueryField(QuestProgressFields)} FROM \"ddon_quest_progress\" WHERE +" +
                                                               $"\"character_common_id\" = @character_common_id AND \"quest_type\" = @quest_type;";
        private readonly string SqlSelectQuestProgressById = $"SELECT {BuildQueryField(QuestProgressFields)} FROM \"ddon_quest_progress\" WHERE +" +
                                                               $"\"character_common_id\" = @character_common_id AND \"quest_schedule_id\" = @quest_schedule_id;";
        private readonly string SqlSelectAllQuestProgress = $"SELECT {BuildQueryField(QuestProgressFields)} FROM \"ddon_quest_progress\" WHERE +" +
                                                               $"\"character_common_id\" = @character_common_id;";

        public List<QuestProgress> GetQuestProgressByType(uint characterCommonId, QuestType questType)
        {
            using TCon connection = OpenNewConnection();
            if (questType == QuestType.All)
            {
                return GetAllQuestProgress(connection, characterCommonId);
            }
            else
            {
                return GetQuestProgressByType(connection, characterCommonId, questType);
            }
        }

        public List<QuestProgress> GetQuestProgressByType(TCon connection, uint characterCommonId, QuestType questType)
        {
            List<QuestProgress> results = new List<QuestProgress>();

            ExecuteInTransaction(conn =>
            {
                ExecuteReader(conn, SqlSelectQuestProgressByType,
                    command =>
                    {
                        AddParameter(command, "@character_common_id", characterCommonId);
                        AddParameter(command, "@quest_type", (uint)questType);
                    }, reader =>
                    {
                        while (reader.Read())
                        {
                            var result = ReadQuestProgress(reader);
                            results.Add(result);
                        }
                    });
            });

            return results;
        }

        private List<QuestProgress> GetAllQuestProgress(TCon connection, uint characterCommonId)
        {
            List<QuestProgress> results = new List<QuestProgress>();

            ExecuteInTransaction(conn =>
            {
                ExecuteReader(conn, SqlSelectAllQuestProgress,
                    command =>
                    {
                        AddParameter(command, "@character_common_id", characterCommonId);
                    }, reader =>
                    {
                        while (reader.Read())
                        {
                            var result = ReadQuestProgress(reader);
                            results.Add(result);
                        }
                    });
            });

            return results;
        }


        public QuestProgress GetQuestProgressById(uint characterCommonId, uint questScheduleId)
        {
            using TCon connection = OpenNewConnection();
            return GetQuestProgressById(connection, characterCommonId, questScheduleId);
        }

        public QuestProgress GetQuestProgressById(TCon connection, uint characterCommonId, uint questScheduleId)
        {
            QuestProgress result = null;
            ExecuteInTransaction(connection =>
            {
                ExecuteReader(connection, SqlSelectQuestProgressById,
                    command =>
                    {
                        AddParameter(command, "@character_common_id", characterCommonId);
                        AddParameter(command, "@quest_schedule_id", questScheduleId);
                    },
                    reader =>
                    {
                        if (reader.Read())
                        {
                            result = ReadQuestProgress(reader);
                        }
                    });
            });
            return result;
        }

        public bool RemoveQuestProgress(uint characterCommonId, uint questScheduleId, QuestType questType)
        {
            using TCon connection = OpenNewConnection();
            return RemoveQuestProgress(connection, characterCommonId, questScheduleId, questType);
        }

        public bool RemoveQuestProgress(TCon connection, uint characterCommonId, uint questScheduleId, QuestType questType)
        {
            return ExecuteNonQuery(connection, SqlDeleteQuestProgress, command =>
            {
                AddParameter(command, "character_common_id", characterCommonId);
                AddParameter(command, "quest_type", (uint)questType);
                AddParameter(command, "quest_schedule_id", questScheduleId);
            }) == 1;
        }

        public bool InsertQuestProgress(uint characterCommonId, uint questScheduleId, QuestType questType, uint step)
        {
            using TCon connection = OpenNewConnection();
            return InsertQuestProgress(connection, characterCommonId, questScheduleId, questType, step);
        }

        public bool InsertQuestProgress(TCon connection, uint characterCommonId, uint questScheduleId, QuestType questType, uint step)
        {
                return ExecuteNonQuery(connection, SqlInsertQuestProgress, command =>
                {
                    AddParameter(command, "character_common_id", characterCommonId);
                    AddParameter(command, "quest_schedule_id", questScheduleId);
                    AddParameter(command, "quest_type", (uint)questType);
                    AddParameter(command, "step", (uint)step);
                }) == 1;
        }

        public bool UpdateQuestProgress(uint characterCommonId, uint questScheduleId, QuestType questType, uint step)
        {
            using TCon connection = OpenNewConnection();
            return UpdateQuestProgress(connection, characterCommonId, questScheduleId, questType, step);
        }

        public bool UpdateQuestProgress(TCon connection, uint characterCommonId, uint questScheduleId, QuestType questType, uint step)
        {
            return ExecuteNonQuery(connection, SqlUpdateQuestProgress, command =>
            {
                AddParameter(command, "character_common_id", characterCommonId);
                AddParameter(command, "quest_schedule_id", questScheduleId);
                AddParameter(command, "quest_type", (uint)questType);
                AddParameter(command, "step", (uint)step);
            }) == 1; ;
        }

        private QuestProgress ReadQuestProgress(TReader reader)
        {
            QuestProgress obj = new QuestProgress();
            obj.CharacterCommonId = GetUInt32(reader, "character_common_id");
            obj.QuestScheduleId = GetUInt32(reader, "quest_schedule_id");
            obj.QuestType = (QuestType)GetUInt32(reader, "quest_type");
            obj.Step = GetUInt32(reader, "step");
            return obj;
        }
    }
}

