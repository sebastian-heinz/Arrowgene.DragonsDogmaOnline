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

        /* ddon_job_master_active_orders */
        protected static readonly string[] JobMasterActiveOrdersField = new string[]
        {
            "character_id", "job_id", "release_type", "release_id", "release_level", "order_accepted"
        };

        private readonly string SqlSelectJobMasterActiveOrders = $"SELECT {BuildQueryField(JobMasterActiveOrdersField)} FROM \"ddon_job_master_active_orders\" WHERE \"character_id\"=@character_id AND \"job_id\"=@job_id;";
        private readonly string SqlSelectJobMasterActiveOrder = $"SELECT {BuildQueryField(JobMasterActiveOrdersField)} FROM \"ddon_job_master_active_orders\" WHERE  \"character_id\"=@character_id AND \"job_id\"=@job_id AND \"release_type\"=@release_type AND \"release_id\"=@release_id;";
        private readonly string SqlInsertJobMasterActiveOrder = $"INSERT INTO \"ddon_job_master_active_orders\" ({BuildQueryField(JobMasterActiveOrdersField)}) VALUES ({BuildQueryInsert(JobMasterActiveOrdersField)});";
        private readonly string SqlUpdateJobMasterActiveOrder = $"UPDATE \"ddon_job_master_active_orders\" SET {BuildQueryUpdate(JobMasterActiveOrdersField)} WHERE \"character_id\"=@character_id AND \"job_id\"=@job_id AND \"release_type\"=@release_type AND \"release_id\"=@release_id;";
        private readonly string SqlDeleteJobMasterActiveOrder = $"DELETE FROM \"ddon_job_master_active_orders\" WHERE \"character_id\"=@character_id AND \"job_id\"=@job_id AND \"release_type\"=@release_type AND \"release_id\"=@release_id;";


        public bool InsertJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe<bool>(connectionIn, (connection) =>
            {
                return ExecuteNonQuery(connection, SqlInsertJobMasterActiveOrder, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "job_id", (byte)jobId);
                    AddParameter(command, "release_type", (byte)activeJobOrder.ReleaseType);
                    AddParameter(command, "release_id", activeJobOrder.ReleaseId);
                    AddParameter(command, "release_level", activeJobOrder.ReleaseLv);
                    AddParameter(command, "order_accepted", activeJobOrder.OrderAccepted);
                }) == 1;
            });
        }

        public bool UpdateJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe<bool>(connectionIn, (connection) =>
            {
                return ExecuteNonQuery(connection, SqlUpdateJobMasterActiveOrder, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "job_id", (byte)jobId);
                    AddParameter(command, "release_type", (byte)activeJobOrder.ReleaseType);
                    AddParameter(command, "release_id", activeJobOrder.ReleaseId);
                    AddParameter(command, "release_level", activeJobOrder.ReleaseLv);
                    AddParameter(command, "order_accepted", activeJobOrder.OrderAccepted);
                }) == 1;
            });
        }

        public bool HasJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null)
        {
            bool foundRecord = false;
            ExecuteQuerySafe(connectionIn, (connection) =>
            {
                ExecuteReader(connection, SqlSelectJobMasterActiveOrder, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "job_id", (byte)jobId);
                    AddParameter(command, "release_type", (byte)activeJobOrder.ReleaseType);
                    AddParameter(command, "release_id", activeJobOrder.ReleaseId);
                }, reader =>
                {
                    foundRecord = reader.Read();
                });
            });
            return foundRecord;
        }

        public bool UpsertJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null)
        {
            return HasJobMasterActiveOrder(characterId, jobId, activeJobOrder, connectionIn) ?
                UpdateJobMasterActiveOrder(characterId, jobId, activeJobOrder, connectionIn) :
                InsertJobMasterActiveOrder(characterId, jobId, activeJobOrder, connectionIn);
        }

        public CDataActiveJobOrder GetJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null)
        {
            CDataActiveJobOrder result = null;
            ExecuteQuerySafe(connectionIn, (connection) =>
            {
                ExecuteReader(connection, SqlSelectJobMasterActiveOrder, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "job_id", (byte)jobId);
                    AddParameter(command, "release_type", (byte)activeJobOrder.ReleaseType);
                    AddParameter(command, "release_id", activeJobOrder.ReleaseId);
                }, reader =>
                {
                    if (reader.Read())
                    {
                        result = new CDataActiveJobOrder()
                        {
                            ReleaseId = GetUInt32(reader, "release_id"),
                            ReleaseType = (ReleaseType)GetByte(reader, "release_type"),
                            ReleaseLv = GetByte(reader, "release_level"),
                            OrderAccepted = GetBoolean(reader, "order_accepted"),
                        };
                    }
                });
            });
            return result;
        }

        public List<CDataActiveJobOrder> GetJobMasterActiveOrders(uint characterId, JobId jobId, DbConnection? connectionIn = null)
        {
            var results = new List<CDataActiveJobOrder>();
            ExecuteQuerySafe(connectionIn, (connection) =>
            {
                ExecuteReader(connection, SqlSelectJobMasterActiveOrders, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "job_id", (byte)jobId);
                }, reader =>
                {
                    while (reader.Read())
                    {
                        results.Add(new CDataActiveJobOrder()
                        {
                            ReleaseId = GetUInt32(reader, "release_id"),
                            ReleaseType = (ReleaseType)GetByte(reader, "release_type"),
                            ReleaseLv = GetByte(reader, "release_level"),
                            OrderAccepted = GetBoolean(reader, "order_accepted"),
                        });
                    }
                });
            });
            return results;
        }

        public bool DeleteJobMasterActiveOrder(uint characterId, JobId jobId, CDataActiveJobOrder activeJobOrder, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe<bool>(connectionIn, (connection) =>
            {
                return ExecuteNonQuery(connection, SqlDeleteJobMasterActiveOrder, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "job_id", (byte)jobId);
                    AddParameter(command, "release_type", (byte)activeJobOrder.ReleaseType);
                    AddParameter(command, "release_id", activeJobOrder.ReleaseId);
                }) == 1;
            });
        }
    }
}
