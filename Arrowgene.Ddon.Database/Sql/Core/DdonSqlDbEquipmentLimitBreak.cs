using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    /* ddon_equipment_limit_break */
    protected static readonly string[] EquipmentLimitBreakFields = new[]
    {
        "character_id", "item_uid", "effect_1", "effect_2", "is_effect1_valid", "is_effect2_valid"
    };

    // Identify key vs. non‑key fields for the upsert
    protected static readonly string[] EquipmentLimitBreakKeyFields = new[]
    {
        "character_id", "item_uid"
    };

    protected static readonly string[] EquipmentLimitBreakNonKeyFields
        = EquipmentLimitBreakFields.Except(EquipmentLimitBreakKeyFields).ToArray();

    protected readonly string SqlInsertEquipmentLimitBreakRecord =
        $"INSERT INTO \"ddon_equipment_limit_break\" ({BuildQueryField(EquipmentLimitBreakFields)}) VALUES ({BuildQueryInsert(EquipmentLimitBreakFields)});";

    protected readonly string SqlSelectEquipmentLimitBreakRecord =
        $"SELECT {BuildQueryField(EquipmentLimitBreakFields)} FROM \"ddon_equipment_limit_break\" WHERE \"item_uid\"=@item_uid;";

    protected readonly string SqlUpdateEquipmentLimitBreakRecord =
        $"UPDATE \"ddon_equipment_limit_break\" SET {BuildQueryUpdate(EquipmentLimitBreakFields)} WHERE \"character_id\"=@character_id AND \"item_uid\"=@item_uid;";

    private readonly string SqlUpsertEquipmentLimitBreakRecord =
        $"""
         INSERT INTO "ddon_equipment_limit_break" ({BuildQueryField(EquipmentLimitBreakFields)}) 
                        VALUES ({BuildQueryInsert(EquipmentLimitBreakFields)}) 
                        ON CONFLICT ("character_id","item_uid") 
                        DO UPDATE SET {BuildQueryUpdateWithPrefix("EXCLUDED.", EquipmentLimitBreakNonKeyFields)};
         """;

    public override bool InsertEquipmentLimitBreakRecord(uint characterId, string itemUID, CDataAddStatusParam statusParam, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlInsertEquipmentLimitBreakRecord, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "item_uid", itemUID);
                AddParameter(command, "effect_1", statusParam.AdditionalStatus1);
                AddParameter(command, "effect_2", statusParam.AdditionalStatus2);
                AddParameter(command, "is_effect1_valid", statusParam.IsAddStat1);
                AddParameter(command, "is_effect2_valid", statusParam.IsAddStat2);
            }) == 1;
        });
    }

    public override bool UpdateEquipmentLimitBreakRecord(uint characterId, string itemUID, CDataAddStatusParam statusParam, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlUpdateEquipmentLimitBreakRecord, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "item_uid", itemUID);
                AddParameter(command, "effect_1", statusParam.AdditionalStatus1);
                AddParameter(command, "effect_2", statusParam.AdditionalStatus2);
                AddParameter(command, "is_effect1_valid", statusParam.IsAddStat1);
                AddParameter(command, "is_effect2_valid", statusParam.IsAddStat2);
            }) == 1;
        });
    }

    public override bool HasEquipmentLimitBreakRecord(uint characterId, string itemUID, DbConnection? connectionIn = null)
    {
        bool foundRecord = false;
        ExecuteQuerySafe(connectionIn, connection =>
        {
            ExecuteReader(connection, SqlSelectEquipmentLimitBreakRecord, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "item_uid", itemUID);
            }, reader => { foundRecord = reader.Read(); });
        });
        return foundRecord;
    }

    /// <summary>
    ///     Insert or update in one round‑trip using Postgres ON CONFLICT.
    ///     Returns true if exactly one row was inserted or updated.
    /// </summary>
    public override bool UpsertEquipmentLimitBreakRecord(uint characterId, string itemUID, CDataAddStatusParam statusParam, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlUpsertEquipmentLimitBreakRecord, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "item_uid", itemUID);
                AddParameter(command, "effect_1", statusParam.AdditionalStatus1);
                AddParameter(command, "effect_2", statusParam.AdditionalStatus2);
                AddParameter(command, "is_effect1_valid", statusParam.IsAddStat1);
                AddParameter(command, "is_effect2_valid", statusParam.IsAddStat2);
            }) == 1;
        });
    }

    public override List<CDataAddStatusParam> GetEquipmentLimitBreakRecord(string itemUID, DbConnection? connectionIn = null)
    {
        List<CDataAddStatusParam> results = new();
        ExecuteQuerySafe(connectionIn, connection =>
        {
            ExecuteReader(connection, SqlSelectEquipmentLimitBreakRecord, command => { AddParameter(command, "item_uid", itemUID); }, reader =>
            {
                while (reader.Read())
                    results.Add(new CDataAddStatusParam
                    {
                        AdditionalStatus1 = GetUInt16(reader, "effect_1"),
                        AdditionalStatus2 = GetUInt16(reader, "effect_2"),
                        IsAddStat1 = GetBoolean(reader, "is_effect1_valid"),
                        IsAddStat2 = GetBoolean(reader, "is_effect2_valid")
                    });
            });
        });
        return results;
    }
}
