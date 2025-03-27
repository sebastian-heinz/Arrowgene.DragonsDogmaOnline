using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        /* ddon_equipment_limit_break */
        protected static readonly string[] EquipmentLimitBreakFields = new string[]
        {
            "character_id", "item_uid", "effect_1", "effect_2", "is_effect1_valid", "is_effect2_valid"
        };

        private readonly string SqlSelectEquipmentLimitBreakRecord = $"SELECT {BuildQueryField(EquipmentLimitBreakFields)} FROM \"ddon_equipment_limit_break\" WHERE \"item_uid\"=@item_uid;";
        private readonly string SqlInsertEquipmentLimitBreakRecord = $"INSERT INTO \"ddon_equipment_limit_break\" ({BuildQueryField(EquipmentLimitBreakFields)}) VALUES ({BuildQueryInsert(EquipmentLimitBreakFields)});";
        private readonly string SqlUpdateEquipmentLimitBreakRecord = $"UPDATE \"ddon_equipment_limit_break\" SET {BuildQueryUpdate(EquipmentLimitBreakFields)} WHERE \"character_id\"=@character_id AND \"item_uid\"=@item_uid;";

        public bool InsertEquipmentLimitBreakRecord(uint characterId, string itemUID, CDataAddStatusParam statusParam, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe<bool>(connectionIn, (connection) =>
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

        public bool UpdateEquipmentLimitBreakRecord(uint characterId, string itemUID, CDataAddStatusParam statusParam, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe<bool>(connectionIn, (connection) =>
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

        public bool HasEquipmentLimitBreakRecord(uint characterId, string itemUID, DbConnection? connectionIn = null)
        {
            bool foundRecord = false;
            ExecuteQuerySafe(connectionIn, (connection) =>
            {
                ExecuteReader(connection, SqlSelectEquipmentLimitBreakRecord, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "item_uid", itemUID);
                }, reader =>
                {
                    foundRecord = reader.Read();
                });
            });
            return foundRecord;
        }

        public bool UpsertEquipmentLimitBreakRecord(uint characterId, string itemUID, CDataAddStatusParam statusParam, DbConnection? connectionIn = null)
        {
            return HasEquipmentLimitBreakRecord(characterId, itemUID, connectionIn) ?
                UpdateEquipmentLimitBreakRecord(characterId, itemUID, statusParam, connectionIn) :
                InsertEquipmentLimitBreakRecord(characterId, itemUID, statusParam, connectionIn);
        }

        public List<CDataAddStatusParam> GetEquipmentLimitBreakRecord(string itemUID, DbConnection? connectionIn = null)
        {
            var results = new List<CDataAddStatusParam>();
            ExecuteQuerySafe(connectionIn, (connection) =>
            {
                ExecuteReader(connection, SqlSelectEquipmentLimitBreakRecord, command =>
                {
                    AddParameter(command, "item_uid", itemUID);
                }, reader =>
                {
                    while (reader.Read())
                    {
                        results.Add(new CDataAddStatusParam()
                        {
                            AdditionalStatus1 = GetUInt16(reader, "effect_1"),
                            AdditionalStatus2 = GetUInt16(reader, "effect_2"),
                            IsAddStat1 = GetBoolean(reader, "is_effect1_valid"),
                            IsAddStat2 = GetBoolean(reader, "is_effect2_valid"),
                        });
                    }
                });
            });
            return results;
        }
    }
}
