using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Model.Scheduler;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    protected static readonly string[] ScheduleNextFields = new[]
    {
        "type", "timestamp"
    };

    private static readonly string SqlUpdateScheduleNext = "UPDATE \"ddon_schedule_next\" SET \"timestamp\"=@timestamp WHERE \"type\"=@type;";
    private static readonly string SqlSelectScheduleNext = $"SELECT {BuildQueryField(ScheduleNextFields)} FROM \"ddon_schedule_next\";";


    public override Dictionary<TaskType, SchedulerTaskEntry> SelectAllTaskEntries()
    {
        Dictionary<TaskType, SchedulerTaskEntry> results = new();
        ExecuteReader(SqlSelectScheduleNext, command => { }, reader =>
        {
            while (reader.Read())
            {
                TaskType type = (TaskType)GetUInt32(reader, "type");
                results[type] = new SchedulerTaskEntry
                {
                    Type = type,
                    Timestamp = GetInt64(reader, "timestamp")
                };
            }
        });
        return results;
    }

    public override bool UpdateScheduleInfo(TaskType type, long timestamp)
    {
        return ExecuteNonQuery(SqlUpdateScheduleNext, command =>
        {
            AddParameter(command, "@type", (uint)type);
            AddParameter(command, "@timestamp", timestamp);
        }) == 1;
    }
}
