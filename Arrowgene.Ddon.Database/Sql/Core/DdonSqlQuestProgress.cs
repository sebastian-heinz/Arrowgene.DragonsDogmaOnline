using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Xml;
using Arrowgene.Ddon.Shared.Model.Quest;

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
            "character_common_id", "quest_type", "quest_id", "step"
        };

        private readonly string SqlInsertQuestProgress = $"INSERT INTO \"ddon_quest_progress\" ({BuildQueryField(QuestProgressFields)}) VALUES ({BuildQueryInsert(QuestProgressFields)});";
        private readonly string SqlDeleteQuestProgress = $"DELETE FROM \"ddon_quest_progress\" WHERE \"character_common_id\"=@character_common_id AND \"quest_type\"=@quest_type AND \"quest_id\"=@quest_id;";

        private readonly string SqlSelectQuestProgressByType = $"SELECT {BuildQueryField(QuestProgressFields)} FROM \"ddon_quest_progress\" WHERE +" +
                                                               $"\"character_common_id\" = @character_common_id AND \"quest_type\" = @quest_type;";

        public QuestId GetCurrentMsqId(uint characterCommonId)
        {
            var results = GetQuestProgressByType(characterCommonId, QuestType.Main);
            if (results.Count == 0)
            {
                return QuestId.None;
            }
            return results[0].QuestId;
        }

        public List<QuestProgress> GetQuestProgressByType(uint characterCommonId, QuestType questType)
        {
            using TCon connection = OpenNewConnection();
            return GetQuestProgressByType(connection, characterCommonId, questType);
        }

        public List<QuestProgress> GetQuestProgressByType(TCon connection, uint characterCommonId, QuestType questType)
        {
            List<QuestProgress> results = new List<QuestProgress>();

            ExecuteInTransaction(conn =>
            {
                ExecuteReader(conn, SqlSelectQuestProgressByType,
                    command => {
                        AddParameter(command, "@character_common_id", characterCommonId);
                        AddParameter(command, "@quest_type", (uint)questType);
                    }, reader => {
                        while (reader.Read())
                        {
                            var result = ReadQuestProgressByType(reader);
                            results.Add(result);
                        }
                    });
            });

            return results;
        }

        public bool RemoveQuestProgress(uint characterCommonId, QuestType questType, QuestId questId)
        {
            using TCon connection = OpenNewConnection();
            return RemoveQuestProgress(connection, characterCommonId, questType, questId);
        }

        public bool RemoveQuestProgress(TCon connection, uint characterCommonId, QuestType questType, QuestId questId)
        {
            return ExecuteNonQuery(connection, SqlDeleteQuestProgress, command =>
            {
                AddParameter(command, "character_common_id", characterCommonId);
                AddParameter(command, "quest_type", (uint) questType);
                AddParameter(command, "quest_id", (uint) questId);
            }) == 1;
        }

        public bool InsertQuestProgress(uint characterCommonId, QuestId questId, QuestType questType, uint step)
        {
            using TCon connection = OpenNewConnection();
            return InsertQuestProgress(connection, characterCommonId, questId, questType, step);
        }

        public bool InsertQuestProgress(TCon connection, uint characterCommonId, QuestId questId, QuestType questType, uint step)
        {
            return ExecuteNonQuery(connection, SqlInsertQuestProgress, command =>
            {
                AddParameter(command, "character_common_id", characterCommonId);
                AddParameter(command, "quest_id", (uint)questId);
                AddParameter(command, "quest_type", (uint)questType);
                AddParameter(command, "step", (uint) step);
            }) == 1;
        }

        private QuestProgress ReadQuestProgressByType(TReader reader)
        {
            QuestProgress obj = new QuestProgress();
            obj.CharacterCommonId = GetUInt32(reader, "character_common_id");
            obj.QuestId = (QuestId)GetUInt32(reader, "quest_id");
            obj.QuestType = (QuestType)GetUInt32(reader, "quest_type");
            obj.Step = GetUInt32(reader, "step");
            return obj;
        }
    }
}

