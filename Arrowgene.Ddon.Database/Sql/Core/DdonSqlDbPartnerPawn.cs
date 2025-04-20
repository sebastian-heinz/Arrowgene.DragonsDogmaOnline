using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    /* ddon_partner_pawn */
    protected static readonly string[] PartnerPawnFields = new[]
    {
        "character_id", "pawn_id", "num_gifts", "num_crafts", "num_adventures"
    };

    private readonly string SqlInsertPartnerPawnRecord =
        $"INSERT INTO \"ddon_partner_pawn\" ({BuildQueryField(PartnerPawnFields)}) VALUES ({BuildQueryInsert(PartnerPawnFields)});";

    private readonly string SqlSelectPartnerPawnRecord =
        $"SELECT {BuildQueryField(PartnerPawnFields)} FROM \"ddon_partner_pawn\" WHERE \"character_id\"=@character_id AND \"pawn_id\" = @pawn_id;";

    private readonly string SqlUpdatePartnerPawnRecord =
        $"UPDATE \"ddon_partner_pawn\" SET {BuildQueryUpdate(PartnerPawnFields)} WHERE \"character_id\" = @character_id AND \"pawn_id\" = @pawn_id;";

    public override bool InsertPartnerPawnRecord(uint characterId, PartnerPawnData partnerPawnData, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlInsertPartnerPawnRecord, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "pawn_id", partnerPawnData.PawnId);
                AddParameter(command, "num_gifts", partnerPawnData.NumGifts);
                AddParameter(command, "num_crafts", partnerPawnData.NumCrafts);
                AddParameter(command, "num_adventures", partnerPawnData.NumAdventures);
            }) == 1;
        });
    }

    public override bool UpdatePartnerPawnRecord(uint characterId, PartnerPawnData partnerPawnData, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlUpdatePartnerPawnRecord, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "pawn_id", partnerPawnData.PawnId);
                AddParameter(command, "num_gifts", partnerPawnData.NumGifts);
                AddParameter(command, "num_crafts", partnerPawnData.NumCrafts);
                AddParameter(command, "num_adventures", partnerPawnData.NumAdventures);
            }) == 1;
        });
    }

    public override PartnerPawnData GetPartnerPawnRecord(uint characterId, uint pawnId, DbConnection? connectionIn = null)
    {
        PartnerPawnData result = null;
        ExecuteQuerySafe(connectionIn, connection =>
        {
            ExecuteReader(connection, SqlSelectPartnerPawnRecord, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "pawn_id", pawnId);
            }, reader =>
            {
                if (reader.Read())
                    result = new PartnerPawnData
                    {
                        PawnId = GetUInt32(reader, "pawn_id"),
                        NumGifts = GetUInt32(reader, "num_gifts"),
                        NumCrafts = GetUInt32(reader, "num_crafts"),
                        NumAdventures = GetUInt32(reader, "num_adventures")
                    };
            });
        });
        return result;
    }

    public override bool SetPartnerPawn(uint characterId, uint pawnId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlUpdatePartnerPawnId, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "partner_pawn_id", pawnId);
            }) == 1;
        });
    }
}
