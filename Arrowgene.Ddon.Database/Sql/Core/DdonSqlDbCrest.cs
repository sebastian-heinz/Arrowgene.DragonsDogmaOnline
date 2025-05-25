#nullable enable
using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    /* ddon_completed_quests */
    protected static readonly string[] CrestFields = new[]
    {
        "character_common_id", "item_uid", "slot", "crest_id", "crest_amount"
    };

    private readonly string SqlDeleteCrestData =
        "DELETE FROM \"ddon_crests\" WHERE \"character_common_id\"=@character_common_id AND \"item_uid\"=@item_uid AND \"slot\"=@slot;";

    private readonly string SqlInsertCrestData = $"INSERT INTO \"ddon_crests\" ({BuildQueryField(CrestFields)}) VALUES ({BuildQueryInsert(CrestFields)});";

    private readonly string SqlSelectAllCrestData = $"SELECT {BuildQueryField(CrestFields)} FROM \"ddon_crests\" WHERE " +
                                                    $"\"character_common_id\" = @character_common_id AND \"item_uid\"=@item_uid;";

    private readonly string SqlSelectAllCrestDataByUid = $"SELECT {BuildQueryField(CrestFields)} FROM \"ddon_crests\" WHERE " +
                                                         $"\"item_uid\"=@item_uid;";

    private readonly string SqlUpdateCrestData =
        $"UPDATE \"ddon_crests\" SET {BuildQueryUpdate(CrestFields)} WHERE \"character_common_id\"=@character_common_id AND \"item_uid\"=@item_uid AND \"slot\"=@slot;";


    public override bool InsertCrest(uint characterCommonId, string itemUId, uint slot, uint crestId, uint crestAmount, DbConnection? connectionIn = null)
    {
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            return ExecuteNonQuery(connection, SqlInsertCrestData, command =>
            {
                AddParameter(command, "character_common_id", characterCommonId);
                AddParameter(command, "item_uid", itemUId);
                AddParameter(command, "slot", slot);
                AddParameter(command, "crest_id", crestId);
                AddParameter(command, "crest_amount", crestAmount);
            }) == 1;
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }
    }

    public override bool UpdateCrest(uint characterCommonId, string itemUId, uint slot, uint crestId, uint crestAmount, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlUpdateCrestData, command =>
            {
                AddParameter(command, "character_common_id", characterCommonId);
                AddParameter(command, "item_uid", itemUId);
                AddParameter(command, "slot", slot);
                AddParameter(command, "crest_id", crestId);
                AddParameter(command, "crest_amount", crestAmount);
            }) == 1;
        });
    }

    public override bool RemoveCrest(uint characterCommonId, string itemUId, uint slot, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlDeleteCrestData, command =>
            {
                AddParameter(command, "character_common_id", characterCommonId);
                AddParameter(command, "item_uid", itemUId);
                AddParameter(command, "slot", slot);
            }) == 1;
        });
    }

    public override List<Crest> GetCrests(uint characterCommonId, string itemUId)
    {
        using DbConnection connection = OpenNewConnection();
        return GetCrests(connection, characterCommonId, itemUId);
    }

    public List<Crest> GetCrests(DbConnection connection, uint characterCommonId, string itemUId)
    {
        List<Crest> results = new();

        ExecuteInTransaction(connection =>
        {
            ExecuteReader(connection, SqlSelectAllCrestData,
                command =>
                {
                    AddParameter(command, "character_common_id", characterCommonId);
                    AddParameter(command, "item_uid", itemUId);
                }, reader =>
                {
                    while (reader.Read())
                    {
                        Crest result = ReadCrestData(reader);
                        results.Add(result);
                    }
                });
        });

        return results;
    }

    private Crest ReadCrestData(DbDataReader reader)
    {
        Crest obj = new();
        obj.Slot = GetUInt32(reader, "slot");
        obj.CrestId = GetUInt32(reader, "crest_id");
        obj.Amount = GetUInt32(reader, "crest_amount");
        return obj;
    }
}
