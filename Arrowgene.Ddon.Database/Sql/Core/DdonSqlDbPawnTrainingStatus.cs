using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        protected static readonly string[] PawnTrainingStatusFields = new string[]
        {
            "pawn_id", "job", "training_status"
        };

        private readonly string SqlInsertPawnTrainingStatus =
            $"INSERT INTO \"ddon_pawn_training_status\" ({BuildQueryField(PawnTrainingStatusFields)}) VALUES ({BuildQueryInsert(PawnTrainingStatusFields)});";

        protected virtual string SqlInsertIfNotExistsPawnTrainingStatus { get; } =
            $"INSERT INTO \"ddon_pawn_training_status\" ({BuildQueryField(PawnTrainingStatusFields)}) SELECT {BuildQueryInsert(PawnTrainingStatusFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_pawn_training_status\" WHERE \"pawn_id\" = @pawn_id AND \"job\" = @job);";

        private static readonly string SqlUpdatePawnTrainingStatus =
            $"UPDATE \"ddon_pawn_training_status\" SET {BuildQueryUpdate(PawnTrainingStatusFields)} WHERE \"pawn_id\" = @pawn_id AND \"job\" = @job;";

        private static readonly string SqlSelectPawnTrainingStatus =
            $"SELECT {BuildQueryField(PawnTrainingStatusFields)} FROM \"ddon_pawn_training_status\" WHERE \"pawn_id\" = @pawn_id AND \"job\" = @job;";

        private static readonly string SqlSelectPawnTrainingStatusByPawn =
            $"SELECT {BuildQueryField(PawnTrainingStatusFields)} FROM \"ddon_pawn_training_status\" WHERE \"pawn_id\" = @pawn_id;";

        private const string SqlDeletePawnTrainingStatus = "DELETE FROM \"ddon_pawn_training_status\" WHERE \"pawn_id\"=@pawn_id AND \"job\" = @job;";

        public bool ReplacePawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                Logger.Debug("Inserting pawn training status.");
                if (!InsertIfNotExistsPawnTrainingStatus(connection, pawnId, job, pawnTrainingStatus))
                {
                    Logger.Debug("Character pawn training status, replacing.");
                    return UpdatePawnTrainingStatus(connection, pawnId, job, pawnTrainingStatus);
                }
                return true;
            });
        }

        public bool InsertPawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus)
        {
            using TCon connection = OpenNewConnection();
            return InsertPawnTrainingStatus(connection, pawnId, job, pawnTrainingStatus);
        }

        public bool InsertPawnTrainingStatus(TCon connection, uint pawnId, JobId job, byte[] pawnTrainingStatus)
        {
            return ExecuteNonQuery(connection, SqlInsertPawnTrainingStatus, command => {
                AddParameter(command, "@pawn_id", pawnId);
                AddParameter(command, "@job", (byte) job);
                AddParameter(command, "@training_status", pawnTrainingStatus);
            }) == 1;
        }
        
        public bool InsertIfNotExistsPawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus)
        {
            using TCon connection = OpenNewConnection();
            return InsertIfNotExistsPawnTrainingStatus(connection, pawnId, job, pawnTrainingStatus);
        }

        public bool InsertIfNotExistsPawnTrainingStatus(TCon connection, uint pawnId, JobId job, byte[] pawnTrainingStatus)
        {
            return ExecuteNonQuery(connection, SqlInsertIfNotExistsPawnTrainingStatus, command => {
                AddParameter(command, "@pawn_id", pawnId);
                AddParameter(command, "@job", (byte) job);
                AddParameter(command, "@training_status", pawnTrainingStatus);
            }) == 1;
        }

        public bool UpdatePawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus)
        {
            using TCon connection = OpenNewConnection();
            return UpdatePawnTrainingStatus(connection, pawnId, job, pawnTrainingStatus);
        }

        public bool UpdatePawnTrainingStatus(TCon connection, uint pawnId, JobId job, byte[] pawnTrainingStatus)
        {
            return ExecuteNonQuery(connection, SqlUpdatePawnTrainingStatus, command => {
                AddParameter(command, "@pawn_id", pawnId);
                AddParameter(command, "@job", (byte) job);
                AddParameter(command, "@training_status", pawnTrainingStatus);
            }) == 1;
        }
    }
}
