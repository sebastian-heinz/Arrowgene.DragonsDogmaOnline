using System.Collections.Generic;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    /* ddon_completed_quests */
    protected static readonly string[] PriorityQuestFields = new[]
    {
        "character_common_id", "quest_schedule_id"
    };

    private readonly string SqlDeletePriorityQuest =
        "DELETE FROM \"ddon_priority_quests\" WHERE \"character_common_id\" = @character_common_id AND \"quest_schedule_id\" = @quest_schedule_id;";

    private readonly string SqlInsertIfNotExistPriorityQuestId = $"INSERT INTO \"ddon_priority_quests\" ({BuildQueryField(PriorityQuestFields)}) SELECT " +
                                                                 $"{BuildQueryInsert(PriorityQuestFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_priority_quests\" WHERE " +
                                                                 $"\"character_common_id\" = @character_common_id AND \"quest_schedule_id\" = @quest_schedule_id);";

    private readonly string SqlSelectPriorityQuests =
        $"SELECT {BuildQueryField(PriorityQuestFields)} FROM \"ddon_priority_quests\" WHERE \"character_common_id\" = @character_common_id;";

    public override bool InsertPriorityQuest(uint characterCommonId, uint questScheduleId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlInsertIfNotExistPriorityQuestId, command =>
            {
                AddParameter(command, "character_common_id", characterCommonId);
                AddParameter(command, "quest_schedule_id", questScheduleId);
            }) == 1;
        });
    }

    public override List<uint> GetPriorityQuestScheduleIds(uint characterCommonId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            List<uint> results = new();
            ExecuteReader(connection, SqlSelectPriorityQuests,
                command => { AddParameter(command, "@character_common_id", characterCommonId); }, reader =>
                {
                    while (reader.Read()) results.Add(GetUInt32(reader, "quest_schedule_id"));
                });
            return results;
        });
    }

    public override bool DeletePriorityQuest(uint characterCommonId, uint questScheduleId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlDeletePriorityQuest, command =>
            {
                AddParameter(command, "@character_common_id", characterCommonId);
                AddParameter(command, "quest_schedule_id", questScheduleId);
            }) == 1;
        });
    }
}
