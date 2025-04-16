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
        /* ddon_job_master_released_elements */
        protected static readonly string[] JobMasterReleasedElementFields = new string[]
        {
            "character_id", "job_id", "release_type", "release_id", "release_level"
        };

        private readonly string SqlSelectJobMasterReleasedElements = $"SELECT {BuildQueryField(JobMasterReleasedElementFields)} FROM \"ddon_job_master_released_elements\" WHERE \"character_id\"=@character_id AND \"job_id\"=@job_id;";
        private readonly string SqlSelectJobMasterReleasedElement = $"SELECT {BuildQueryField(JobMasterReleasedElementFields)} FROM \"ddon_job_master_released_elements\" WHERE  \"character_id\"=@character_id AND \"job_id\"=@job_id AND \"release_type\"=@release_type AND \"release_id\"=@release_id AND \"release_level\"=@release_level;";
        private readonly string SqlInsertJobMasterReleasedElement = $"INSERT INTO \"ddon_job_master_released_elements\" ({BuildQueryField(JobMasterReleasedElementFields)}) VALUES ({BuildQueryInsert(JobMasterReleasedElementFields)});";

        public bool InsertJobMasterReleasedElement(uint characterId, JobId jobId, CDataReleaseElement releasedElement, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe<bool>(connectionIn, (connection) =>
            {
                return ExecuteNonQuery(connection, SqlInsertJobMasterReleasedElement, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "job_id", (byte) jobId);
                    AddParameter(command, "release_type", (byte) releasedElement.ReleaseType);
                    AddParameter(command, "release_id", releasedElement.ReleaseId);
                    AddParameter(command, "release_level", releasedElement.ReleaseLv);
                }) == 1;
            });
        }

        public bool HasJobMasterReleasedElement(uint characterId, JobId jobId, CDataReleaseElement releasedElement, DbConnection? connectionIn = null)
        {
            bool foundRecord = false;
            ExecuteQuerySafe(connectionIn, (connection) =>
            {
                ExecuteReader(connection, SqlSelectJobMasterReleasedElement, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "job_id", (byte)jobId);
                    AddParameter(command, "release_type", (byte)releasedElement.ReleaseType);
                    AddParameter(command, "release_id", releasedElement.ReleaseId);
                    AddParameter(command, "release_level", releasedElement.ReleaseLv);
                }, reader =>
                {
                    foundRecord = reader.Read();
                });
            });
            return foundRecord;
        }

        public CDataReleaseElement GetJobMasterReleasedElement(uint characterId, JobId jobId, CDataReleaseElement releasedElement, DbConnection? connectionIn = null)
        {
            CDataReleaseElement result = null;
            ExecuteQuerySafe(connectionIn, (connection) =>
            {
                ExecuteReader(connection, SqlSelectJobMasterReleasedElement, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "job_id", (byte)jobId);
                    AddParameter(command, "release_type", (byte)releasedElement.ReleaseType);
                    AddParameter(command, "release_id", releasedElement.ReleaseId);
                    AddParameter(command, "release_level", releasedElement.ReleaseLv);
                }, reader =>
                {
                    if (reader.Read())
                    {
                        result = new CDataReleaseElement()
                        {
                            ReleaseId = GetUInt32(reader, "release_id"),
                            ReleaseType = (JobTrainingReleaseType) GetByte(reader, "release_type"),
                            ReleaseLv = GetByte(reader, "release_level"),
                        };
                    }
                });
            });
            return result;
        }

        public List<CDataReleaseElement> GetJobMasterReleasedElements(uint characterId, JobId jobId, DbConnection? connectionIn = null)
        {
            var results = new List<CDataReleaseElement>();
            ExecuteQuerySafe(connectionIn, (connection) =>
            {
                ExecuteReader(connection, SqlSelectJobMasterReleasedElements, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "job_id", (byte)jobId);
                }, reader =>
                {
                    while (reader.Read())
                    {
                        results.Add(new CDataReleaseElement()
                        {
                            ReleaseId = GetUInt32(reader, "release_id"),
                            ReleaseType = (JobTrainingReleaseType)GetByte(reader, "release_type"),
                            ReleaseLv = GetByte(reader, "release_level"),
                        });
                    }
                });
            });
            return results;
        }
    }
}
