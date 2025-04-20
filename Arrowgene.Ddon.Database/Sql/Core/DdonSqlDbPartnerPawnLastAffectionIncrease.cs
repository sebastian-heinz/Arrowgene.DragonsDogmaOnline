using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    /* ddon_partner_pawn_last_affection_increase */
    protected static readonly string[] PartnerPawnLastAffectionIncreaseFields = new[]
    {
        "character_id", "pawn_id", "action"
    };

    private readonly string SqlDeleteDailyPartnerPawnLastAffectionIncreaseRecords = "DELETE FROM ddon_partner_pawn_last_affection_increase;";

    private readonly string SqlInsertPartnerPawnLastAffectionIncreaseRecord =
        $"INSERT INTO \"ddon_partner_pawn_last_affection_increase\" ({BuildQueryField(PartnerPawnLastAffectionIncreaseFields)}) VALUES ({BuildQueryInsert(PartnerPawnLastAffectionIncreaseFields)});";

    private readonly string SqlSelectPartnerPawnLastAffectionIncreaseRecord =
        $"SELECT {BuildQueryField(PartnerPawnLastAffectionIncreaseFields)} FROM \"ddon_partner_pawn_last_affection_increase\" WHERE \"character_id\"=@character_id AND \"pawn_id\"=@pawn_id AND \"action\"=@action;";

    public override bool InsertPartnerPawnLastAffectionIncreaseRecord(uint characterId, uint pawnId, PartnerPawnAffectionAction action, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlInsertPartnerPawnLastAffectionIncreaseRecord, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "pawn_id", pawnId);
                AddParameter(command, "action", (byte)action);
            }) == 1;
        });
    }

    public override bool HasPartnerPawnLastAffectionIncreaseRecord(uint characterId, uint pawnId, PartnerPawnAffectionAction action, DbConnection? connectionIn = null)
    {
        bool foundRecord = false;
        ExecuteQuerySafe(connectionIn, connection =>
        {
            ExecuteReader(connection, SqlSelectPartnerPawnLastAffectionIncreaseRecord, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "pawn_id", pawnId);
                AddParameter(command, "action", (byte)action);
            }, reader => { foundRecord = reader.Read(); });
        });
        return foundRecord;
    }

    public override void DeleteAllPartnerPawnLastAffectionIncreaseRecords(DbConnection? connectionIn = null)
    {
        ExecuteQuerySafe(connectionIn, connection => { ExecuteNonQuery(connection, SqlDeleteDailyPartnerPawnLastAffectionIncreaseRecords, command => { }); });
    }
}
