using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        /* ddon_job_master_active_orders_progress */
        protected static readonly string[] JobMasterActiveOrdersProgress = new string[]
        {
            "character_id", "job_id", "release_type", "release_id", "condition", "target_id", "target_rank", "target_num", "current_num"
        };

        private readonly string SqlSelectJobMasterActiveOrdersProgress = $"SELECT {BuildQueryField(JobMasterActiveOrdersProgress)} FROM \"ddon_job_master_active_orders_progress\" WHERE  \"character_id\"=@character_id AND \"job_id\"=@job_id AND \"release_type\"=@release_type AND \"release_id\"=@release_id;";
        private readonly string SqlInsertJobMasterActiveOrdersProgress = $"INSERT INTO \"ddon_job_master_active_orders_progress\" ({BuildQueryField(JobMasterActiveOrdersProgress)}) VALUES ({BuildQueryInsert(JobMasterActiveOrdersProgress)});";
        private readonly string SqlUpdateJobMasterActiveOrdersProgress = $"UPDATE \"ddon_job_master_active_orders_progress\" SET {BuildQueryUpdate(JobMasterActiveOrdersProgress)} WHERE \"character_id\"=@character_id AND \"job_id\"=@job_id AND \"release_type\"=@release_type AND \"release_id\"=@release_id;";


        public bool InsertJobMasterActiveOrderProgress(uint characterId, JobId jobId, JobTrainingReleaseType releaseType, uint releaseId, CDataJobOrderProgress jobOrderProgress, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe<bool>(connectionIn, (connection) =>
            {
                return ExecuteNonQuery(connection, SqlInsertJobMasterActiveOrdersProgress, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "job_id", (byte)jobId);
                    AddParameter(command, "release_type", (byte) releaseType);
                    AddParameter(command, "release_id", releaseId);
                    AddParameter(command, "condition", (byte) jobOrderProgress.ConditionType);
                    AddParameter(command, "target_id", jobOrderProgress.TargetId);
                    AddParameter(command, "target_rank", jobOrderProgress.TargetRank);
                    AddParameter(command, "target_num", jobOrderProgress.TargetNum);
                    AddParameter(command, "current_num", jobOrderProgress.CurrentNum);
                }) == 1;
            });
        }

        public bool UpdateJobMasterActiveOrderProgress(uint characterId, JobId jobId, JobTrainingReleaseType releaseType, uint releaseId, CDataJobOrderProgress jobOrderProgress, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe<bool>(connectionIn, (connection) =>
            {
                return ExecuteNonQuery(connection, SqlUpdateJobMasterActiveOrdersProgress, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "job_id", (byte)jobId);
                    AddParameter(command, "release_type", (byte)releaseType);
                    AddParameter(command, "release_id", releaseId);
                    AddParameter(command, "condition", (byte)jobOrderProgress.ConditionType);
                    AddParameter(command, "target_id", jobOrderProgress.TargetId);
                    AddParameter(command, "target_rank", jobOrderProgress.TargetRank);
                    AddParameter(command, "target_num", jobOrderProgress.TargetNum);
                    AddParameter(command, "current_num", jobOrderProgress.CurrentNum);
                }) == 1;
            });
        }

        public bool HasJobMasterActiveOrderProgress(uint characterId, JobId jobId, JobTrainingReleaseType releaseType, uint releaseId, CDataJobOrderProgress jobOrderProgress, DbConnection? connectionIn = null)
        {
            bool foundRecord = false;
            ExecuteQuerySafe(connectionIn, (connection) =>
            {
                ExecuteReader(connection, SqlSelectJobMasterActiveOrdersProgress, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "job_id", (byte)jobId);
                    AddParameter(command, "release_type", (byte)releaseType);
                    AddParameter(command, "release_id", releaseId);
                }, reader =>
                {
                    foundRecord = reader.Read();
                });
            });
            return foundRecord;
        }

        public bool UpsertJobMasterActiveOrdersProgress(uint characterId, JobId jobId, JobTrainingReleaseType releaseType, uint releaseId, CDataJobOrderProgress jobOrderProgress, DbConnection? connectionIn = null)
        {
            return HasJobMasterActiveOrderProgress(characterId, jobId, releaseType, releaseId, jobOrderProgress, connectionIn) ?
                UpdateJobMasterActiveOrderProgress(characterId, jobId, releaseType, releaseId, jobOrderProgress, connectionIn) :
                InsertJobMasterActiveOrderProgress(characterId, jobId, releaseType, releaseId, jobOrderProgress, connectionIn);
        }

        public List<CDataJobOrderProgress> GetJobMasterActiveOrderProgress(uint characterId, JobId jobId, JobTrainingReleaseType releaseType, uint releaseId, DbConnection? connectionIn = null)
        {
            var results = new List<CDataJobOrderProgress>();
            ExecuteQuerySafe(connectionIn, (connection) =>
            {
                ExecuteReader(connection, SqlSelectJobMasterActiveOrdersProgress, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "job_id", (byte)jobId);
                    AddParameter(command, "release_type", (byte)releaseType);
                    AddParameter(command, "release_id", releaseId);
                }, reader =>
                {
                    while (reader.Read())
                    {
                        results.Add(new CDataJobOrderProgress()
                        {
                            ConditionType = (JobOrderCondType) GetByte(reader, "condition"),
                            TargetId = GetUInt32(reader, "target_id"),
                            TargetRank = GetUInt32(reader, "target_rank"),
                            TargetNum = GetUInt32(reader, "target_num"),
                            CurrentNum = GetUInt32(reader, "current_num"),
                        });
                    }
                });
            });
            return results;
        }
    }
}
