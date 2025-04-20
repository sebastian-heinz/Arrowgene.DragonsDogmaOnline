using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private const string SqlDeletePawnTrainingStatus = "DELETE FROM \"ddon_pawn_training_status\" WHERE \"pawn_id\"=@pawn_id AND \"job\" = @job;";

    protected static readonly string[] PawnTrainingStatusFields = new[]
    {
        "pawn_id", "job", "training_status"
    };

    private static readonly string SqlUpdatePawnTrainingStatus =
        $"UPDATE \"ddon_pawn_training_status\" SET {BuildQueryUpdate(PawnTrainingStatusFields)} WHERE \"pawn_id\" = @pawn_id AND \"job\" = @job;";

    private static readonly string SqlSelectPawnTrainingStatus =
        $"SELECT {BuildQueryField(PawnTrainingStatusFields)} FROM \"ddon_pawn_training_status\" WHERE \"pawn_id\" = @pawn_id AND \"job\" = @job;";

    private static readonly string SqlSelectPawnTrainingStatusByPawn =
        $"SELECT {BuildQueryField(PawnTrainingStatusFields)} FROM \"ddon_pawn_training_status\" WHERE \"pawn_id\" = @pawn_id;";

    private readonly string SqlInsertPawnTrainingStatus =
        $"INSERT INTO \"ddon_pawn_training_status\" ({BuildQueryField(PawnTrainingStatusFields)}) VALUES ({BuildQueryInsert(PawnTrainingStatusFields)});";

    protected virtual string SqlInsertIfNotExistsPawnTrainingStatus { get; } =
        $"INSERT INTO \"ddon_pawn_training_status\" ({BuildQueryField(PawnTrainingStatusFields)}) SELECT {BuildQueryInsert(PawnTrainingStatusFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_pawn_training_status\" WHERE \"pawn_id\" = @pawn_id AND \"job\" = @job);";

    public override bool ReplacePawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus, DbConnection? connectionIn = null)
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

    public override bool InsertPawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertPawnTrainingStatus(connection, pawnId, job, pawnTrainingStatus);
    }

    public bool InsertPawnTrainingStatus(DbConnection connection, uint pawnId, JobId job, byte[] pawnTrainingStatus)
    {
        return ExecuteNonQuery(connection, SqlInsertPawnTrainingStatus, command =>
        {
            AddParameter(command, "@pawn_id", pawnId);
            AddParameter(command, "@job", (byte)job);
            AddParameter(command, "@training_status", pawnTrainingStatus);
        }) == 1;
    }

    public override bool InsertIfNotExistsPawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertIfNotExistsPawnTrainingStatus(connection, pawnId, job, pawnTrainingStatus);
    }

    public bool InsertIfNotExistsPawnTrainingStatus(DbConnection connection, uint pawnId, JobId job, byte[] pawnTrainingStatus)
    {
        return ExecuteNonQuery(connection, SqlInsertIfNotExistsPawnTrainingStatus, command =>
        {
            AddParameter(command, "@pawn_id", pawnId);
            AddParameter(command, "@job", (byte)job);
            AddParameter(command, "@training_status", pawnTrainingStatus);
        }) == 1;
    }

    public override bool UpdatePawnTrainingStatus(uint pawnId, JobId job, byte[] pawnTrainingStatus)
    {
        using DbConnection connection = OpenNewConnection();
        return UpdatePawnTrainingStatus(connection, pawnId, job, pawnTrainingStatus);
    }

    public bool UpdatePawnTrainingStatus(DbConnection connection, uint pawnId, JobId job, byte[] pawnTrainingStatus)
    {
        return ExecuteNonQuery(connection, SqlUpdatePawnTrainingStatus, command =>
        {
            AddParameter(command, "@pawn_id", pawnId);
            AddParameter(command, "@job", (byte)job);
            AddParameter(command, "@training_status", pawnTrainingStatus);
        }) == 1;
    }
}
