using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    /* ddon_completed_quests */
    protected static readonly string[] CompletedQuestsFields =
    [
        "character_common_id", "quest_type", "quest_id", "clear_count"
    ];

    private readonly string SqlInsertCompletedQuestId =
        $"INSERT INTO \"ddon_completed_quests\" ({BuildQueryField(CompletedQuestsFields)}) VALUES ({BuildQueryInsert(CompletedQuestsFields)});";

    private readonly string SqlInsertIfNotExistCompletedQuestId = $"INSERT INTO \"ddon_completed_quests\" ({BuildQueryField(CompletedQuestsFields)}) SELECT " +
                                                                  $"{BuildQueryInsert(CompletedQuestsFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_completed_quests\" WHERE " +
                                                                  $"\"character_common_id\" = @character_common_id AND \"quest_id\" = @quest_id);";

    private readonly string SqlSelectCompletedQuestById =
        $"SELECT {BuildQueryField(CompletedQuestsFields)} FROM \"ddon_completed_quests\" WHERE \"character_common_id\" = @character_common_id AND \"quest_id\" = @quest_id;";

    private readonly string SqlSelectCompletedQuestByType =
        $"SELECT {BuildQueryField(CompletedQuestsFields)} FROM \"ddon_completed_quests\" WHERE \"character_common_id\" = @character_common_id AND \"quest_type\" = @quest_type;";

    private readonly string SqlUpdateCompletedQuestId =
        "UPDATE \"ddon_completed_quests\" SET \"clear_count\" = @clear_count WHERE \"character_common_id\" = @character_common_id AND \"quest_id\" = @quest_id;";

    public override List<CompletedQuest> GetCompletedQuestsByType(uint characterCommonId, QuestType questType, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            List<CompletedQuest> results = new();
            ExecuteReader(connection, SqlSelectCompletedQuestByType,
                command =>
                {
                    AddParameter(command, "@character_common_id", characterCommonId);
                    AddParameter(command, "@quest_type", (uint)questType);
                }, reader =>
                {
                    while (reader.Read())
                        results.Add(new CompletedQuest
                        {
                            QuestId = (QuestId)GetUInt32(reader, "quest_id"),
                            QuestType = questType,
                            ClearCount = GetUInt32(reader, "clear_count")
                        });
                });
            return results;
        });
    }

    public override CompletedQuest GetCompletedQuestsById(uint characterCommonId, QuestId questId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            CompletedQuest result = null;
            ExecuteReader(connection, SqlSelectCompletedQuestById,
                command =>
                {
                    AddParameter(command, "@character_common_id", characterCommonId);
                    AddParameter(command, "@quest_id", (uint)questId);
                }, reader =>
                {
                    if (reader.Read())
                        result = new CompletedQuest
                        {
                            QuestId = questId,
                            QuestType = (QuestType)GetUInt32(reader, "quest_type"),
                            ClearCount = GetUInt32(reader, "clear_count")
                        };
                });
            return result;
        });
    }

    public override bool InsertIfNotExistCompletedQuest(uint characterCommonId, QuestId questId, QuestType questType, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlInsertIfNotExistCompletedQuestId, command =>
            {
                AddParameter(command, "character_common_id", characterCommonId);
                AddParameter(command, "quest_id", (uint)questId);
                AddParameter(command, "quest_type", (uint)questType);
                AddParameter(command, "clear_count", 1);
            }) == 1;
        });
    }

    public override bool ReplaceCompletedQuest(uint characterCommonId, QuestId questId, QuestType questType, uint count = 1, DbConnection? connectionIn = null)
    {
        if (!InsertIfNotExistCompletedQuest(characterCommonId, questId, questType, connectionIn))
            return UpdateCompletedQuest(characterCommonId, questId, questType, count, connectionIn);

        return true;
    }

    private bool UpdateCompletedQuest(uint characterCommonId, QuestId questId, QuestType questType, uint count = 1, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlUpdateCompletedQuestId, command =>
            {
                AddParameter(command, "character_common_id", characterCommonId);
                AddParameter(command, "quest_id", (uint)questId);
                AddParameter(command, "quest_type", (uint)questType);
                AddParameter(command, "clear_count", count);
            }) == 1;
        });
    }
}
