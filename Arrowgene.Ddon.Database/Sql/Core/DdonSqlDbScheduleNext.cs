using Arrowgene.Ddon.Shared.Model.Scheduler;
using System.Collections.Generic;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        protected static readonly string[] ScheduleNextFields = new string[]
        {
            "type", "timestamp"
        };

        private static readonly string SqlUpdateScheduleNext = $"UPDATE \"ddon_schedule_next\" SET \"timestamp\"=@timestamp WHERE \"type\"=@type;";
        private static readonly string SqlSelectScheduleNext = $"SELECT {BuildQueryField(ScheduleNextFields)} FROM \"ddon_schedule_next\";";


        public Dictionary<TaskType, SchedulerTaskEntry> SelectAllTaskEntries()
        {
            Dictionary<TaskType, SchedulerTaskEntry> results = new Dictionary<TaskType, SchedulerTaskEntry>();
            ExecuteReader(SqlSelectScheduleNext, command => { }, reader =>
            {
                while (reader.Read())
                {
                    TaskType type = (TaskType) GetUInt32(reader, "type");
                    results[type] = new SchedulerTaskEntry()
                    {
                        Type = type,
                        Timestamp = GetInt64(reader, "timestamp")
                    };
                }
            });
            return results;
        }

        public bool UpdateScheduleInfo(TaskType type, long timestamp)
        {
            return ExecuteNonQuery(SqlUpdateScheduleNext, command =>
            {
                AddParameter(command, "@type", (uint) type);
                AddParameter(command, "@timestamp", timestamp);
            }) == 1;
        }
    }
}

