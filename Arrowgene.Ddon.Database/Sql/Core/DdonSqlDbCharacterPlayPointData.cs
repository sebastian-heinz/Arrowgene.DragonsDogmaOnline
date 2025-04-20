using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private const string SqlDeleteCharacterPlayPointData = "DELETE FROM \"ddon_character_playpoint_data\" WHERE \"character_id\"=@character_id AND \"job\" = @job;";

    protected static readonly string[] CDataJobPlayPointFields = new[]
    {
        "character_id", "job", "play_point", "exp_mode"
    };

    private static readonly string SqlUpdateCharacterPlayPointData =
        $"UPDATE \"ddon_character_playpoint_data\" SET {BuildQueryUpdate(CDataJobPlayPointFields)} WHERE \"character_id\" = @character_id AND \"job\" = @job;";

    private static readonly string SqlSelectCharacterPlayPointData =
        $"SELECT {BuildQueryField(CDataJobPlayPointFields)} FROM \"ddon_character_playpoint_data\" WHERE \"character_id\" = @character_id AND \"job\" = @job;";

    private static readonly string SqlSelectCharacterPlayPointDataByCharacter =
        $"SELECT {BuildQueryField(CDataJobPlayPointFields)} FROM \"ddon_character_playpoint_data\" WHERE \"character_id\" = @character_id;";

    private readonly string SqlInsertCharacterPlayPointData =
        $"INSERT INTO \"ddon_character_playpoint_data\" ({BuildQueryField(CDataJobPlayPointFields)}) VALUES ({BuildQueryInsert(CDataJobPlayPointFields)});";

    protected virtual string SqlInsertIfNotExistsCharacterPlayPointData { get; } =
        $"INSERT INTO \"ddon_character_playpoint_data\" ({BuildQueryField(CDataJobPlayPointFields)}) SELECT {BuildQueryInsert(CDataJobPlayPointFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_character_playpoint_data\" WHERE \"character_id\" = @character_id AND \"job\" = @job);";

    public override bool ReplaceCharacterPlayPointData(uint id, CDataJobPlayPoint replacedCharacterPlayPointData, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            Logger.Debug("Inserting character playpoint data.");
            if (!InsertIfNotExistsCharacterPlayPointData(connection, id, replacedCharacterPlayPointData))
            {
                Logger.Debug("Character playpoint data already exists, replacing.");
                return UpdateCharacterPlayPointData(id, replacedCharacterPlayPointData, connection);
            }

            return true;
        });
    }

    public bool InsertCharacterPlayPointData(uint id, CDataJobPlayPoint updatedCharacterPlayPointData)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertCharacterPlayPointData(connection, id, updatedCharacterPlayPointData);
    }

    public bool InsertCharacterPlayPointData(DbConnection connection, uint id, CDataJobPlayPoint updatedCharacterPlayPointData)
    {
        return ExecuteNonQuery(connection, SqlInsertCharacterPlayPointData, command => { AddParameter(command, id, updatedCharacterPlayPointData); }) == 1;
    }

    public bool InsertIfNotExistsCharacterPlayPointData(uint id, CDataJobPlayPoint updatedCharacterPlayPointData)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertIfNotExistsCharacterPlayPointData(connection, id, updatedCharacterPlayPointData);
    }

    public bool InsertIfNotExistsCharacterPlayPointData(DbConnection connection, uint id, CDataJobPlayPoint updatedCharacterPlayPointData)
    {
        return ExecuteNonQuery(connection, SqlInsertIfNotExistsCharacterPlayPointData, command => { AddParameter(command, id, updatedCharacterPlayPointData); }) == 1;
    }

    public override bool UpdateCharacterPlayPointData(uint id, CDataJobPlayPoint updatedCharacterPlayPointData, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn,
            connection => { return ExecuteNonQuery(connection, SqlUpdateCharacterPlayPointData, command => { AddParameter(command, id, updatedCharacterPlayPointData); }) == 1; });
    }

    private void AddParameter(DbCommand command, uint id, CDataJobPlayPoint characterPlayPointData)
    {
        AddParameter(command, "character_id", id);
        AddParameter(command, "job", (byte)characterPlayPointData.Job);
        AddParameter(command, "play_point", characterPlayPointData.PlayPoint.PlayPoint);
        AddParameter(command, "exp_mode", (byte)characterPlayPointData.PlayPoint.ExpMode);
    }

    private CDataJobPlayPoint ReadCharacterPlayPointData(DbDataReader reader)
    {
        CDataJobPlayPoint characterPlayPointData = new();
        characterPlayPointData.Job = (JobId)GetByte(reader, "job");
        characterPlayPointData.PlayPoint.PlayPoint = GetUInt32(reader, "play_point");
        characterPlayPointData.PlayPoint.ExpMode = (ExpMode)GetByte(reader, "exp_mode");

        return characterPlayPointData;
    }
}
