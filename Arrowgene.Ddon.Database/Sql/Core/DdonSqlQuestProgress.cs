using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    /* ddon_quest_progress */
    protected static readonly string[] QuestProgressFields = new[]
    {
        "character_common_id", "quest_type", "quest_schedule_id", "step"
    };

    private readonly string SqlDeleteQuestProgress =
        "DELETE FROM \"ddon_quest_progress\" WHERE \"character_common_id\"=@character_common_id AND \"quest_type\"=@quest_type AND \"quest_schedule_id\"=@quest_schedule_id;";

    private readonly string SqlInsertQuestProgress =
        $"INSERT INTO \"ddon_quest_progress\" ({BuildQueryField(QuestProgressFields)}) VALUES ({BuildQueryInsert(QuestProgressFields)});";

    private readonly string SqlSelectAllQuestProgress = $"SELECT {BuildQueryField(QuestProgressFields)} FROM \"ddon_quest_progress\" WHERE " +
                                                        $"\"character_common_id\" = @character_common_id;";

    private readonly string SqlSelectQuestProgressByScheduleId = $"SELECT {BuildQueryField(QuestProgressFields)} FROM \"ddon_quest_progress\" WHERE " +
                                                                 $"\"character_common_id\" = @character_common_id AND \"quest_schedule_id\" = @quest_schedule_id;";

    private readonly string SqlSelectQuestProgressByType = $"SELECT {BuildQueryField(QuestProgressFields)} FROM \"ddon_quest_progress\" WHERE " +
                                                           $"\"character_common_id\" = @character_common_id AND \"quest_type\" = @quest_type;";

    private readonly string SqlUpdateQuestProgress =
        "UPDATE \"ddon_quest_progress\" SET \"step\"=@step WHERE \"character_common_id\"=@character_common_id AND \"quest_type\"=@quest_type AND \"quest_schedule_id\"=@quest_schedule_id;";

    public override List<QuestProgress> GetQuestProgressByType(uint characterCommonId, QuestType questType, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            if (questType == QuestType.All) return GetAllQuestProgress(connection, characterCommonId);

            return GetQuestProgressByType(connection, characterCommonId, questType);
        });
    }

    private List<QuestProgress> GetQuestProgressByType(DbConnection connection, uint characterCommonId, QuestType questType)
    {
        List<QuestProgress> results = new();

        ExecuteReader(connection, SqlSelectQuestProgressByType,
            command =>
            {
                AddParameter(command, "@character_common_id", characterCommonId);
                AddParameter(command, "@quest_type", (uint)questType);
            }, reader =>
            {
                while (reader.Read())
                {
                    QuestProgress result = ReadQuestProgress(reader);
                    results.Add(result);
                }
            }
        );

        return results;
    }

    private List<QuestProgress> GetAllQuestProgress(DbConnection connection, uint characterCommonId)
    {
        List<QuestProgress> results = new();
        ExecuteReader(connection, SqlSelectAllQuestProgress,
            command => { AddParameter(command, "@character_common_id", characterCommonId); }, reader =>
            {
                while (reader.Read())
                {
                    QuestProgress result = ReadQuestProgress(reader);
                    results.Add(result);
                }
            });
        return results;
    }

    public override QuestProgress GetQuestProgressByScheduleId(uint characterCommonId, uint questScheduleId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            QuestProgress result = null;
            ExecuteReader(connection, SqlSelectQuestProgressByScheduleId,
                command =>
                {
                    AddParameter(command, "@character_common_id", characterCommonId);
                    AddParameter(command, "@quest_schedule_id", questScheduleId);
                },
                reader =>
                {
                    if (reader.Read()) result = ReadQuestProgress(reader);
                }
            );
            return result;
        });
    }

    public override bool RemoveQuestProgress(uint characterCommonId, uint questScheduleId, QuestType questType, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlDeleteQuestProgress, command =>
            {
                AddParameter(command, "character_common_id", characterCommonId);
                AddParameter(command, "quest_type", (uint)questType);
                AddParameter(command, "quest_schedule_id", questScheduleId);
            }) == 1;
        });
    }

    public override bool InsertQuestProgress(uint characterCommonId, uint questScheduleId, QuestType questType, uint step, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlInsertQuestProgress, command =>
            {
                AddParameter(command, "character_common_id", characterCommonId);
                AddParameter(command, "quest_schedule_id", questScheduleId);
                AddParameter(command, "quest_type", (uint)questType);
                AddParameter(command, "step", step);
            }) == 1;
        });
    }

    public override bool UpdateQuestProgress(uint characterCommonId, uint questScheduleId, QuestType questType, uint step, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlUpdateQuestProgress, command =>
            {
                AddParameter(command, "character_common_id", characterCommonId);
                AddParameter(command, "quest_schedule_id", questScheduleId);
                AddParameter(command, "quest_type", (uint)questType);
                AddParameter(command, "step", step);
            }) == 1;
        });
    }

    private QuestProgress ReadQuestProgress(DbDataReader reader)
    {
        QuestProgress obj = new();
        obj.CharacterCommonId = GetUInt32(reader, "character_common_id");
        obj.QuestScheduleId = GetUInt32(reader, "quest_schedule_id");
        obj.QuestType = (QuestType)GetUInt32(reader, "quest_type");
        obj.Step = GetUInt32(reader, "step");
        return obj;
    }
}
