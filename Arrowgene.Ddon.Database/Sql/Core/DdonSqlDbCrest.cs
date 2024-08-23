#nullable enable
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
        /* ddon_completed_quests */
        protected static readonly string[] CrestFields = new string[]
        {
            "character_common_id", "item_uid", "slot", "crest_id", "crest_amount"
        };

        private readonly string SqlInsertCrestData = $"INSERT INTO \"ddon_crests\" ({BuildQueryField(CrestFields)}) VALUES ({BuildQueryInsert(CrestFields)});";
        private readonly string SqlDeleteCrestData = $"DELETE FROM \"ddon_crests\" WHERE \"character_common_id\"=@character_common_id AND \"item_uid\"=@item_uid AND \"slot\"=@slot;";
        private readonly string SqlUpdateCrestData = $"UPDATE \"ddon_crests\" SET {BuildQueryUpdate(CrestFields)} WHERE \"character_common_id\"=@character_common_id AND \"item_uid\"=@item_uid AND \"slot\"=@slot;";
        private readonly string SqlSelectAllCrestData = $"SELECT {BuildQueryField(CrestFields)} FROM \"ddon_crests\" WHERE " +
                                                       $"\"character_common_id\" = @character_common_id AND \"item_uid\"=@item_uid;";

        public bool InsertCrest(uint characterCommonId, string itemUId, uint slot, uint crestId, uint crestAmount, DbConnection? connectionIn = null)
        {
            bool isTransaction = connectionIn is not null;
            TCon connection = (TCon)(connectionIn ?? OpenNewConnection());
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

        public bool UpdateCrest(uint characterCommonId, string itemUId, uint slot, uint crestId, uint crestAmount)
        {
            using TCon connection = OpenNewConnection();
            return UpdateCrest(connection, characterCommonId, itemUId, slot, crestId, crestAmount);
        }

        public bool UpdateCrest(TCon connection, uint characterCommonId, string itemUId, uint slot, uint crestId, uint crestAmount)
        {
            return ExecuteNonQuery(connection, SqlUpdateCrestData, command =>
            {
                AddParameter(command, "character_common_id", characterCommonId);
                AddParameter(command, "item_uid", itemUId);
                AddParameter(command, "slot", slot);
                AddParameter(command, "crest_id", crestId);
                AddParameter(command, "crest_amount", crestAmount);
            }) == 1; ;
        }

        public bool RemoveCrest(uint characterCommonId, string itemUId, uint slot)
        {
            using TCon connection = OpenNewConnection();
            return RemoveCrest(connection, characterCommonId, itemUId, slot);
        }

        public bool RemoveCrest(TCon connection, uint characterCommonId, string itemUId, uint slot)
        {
            return ExecuteNonQuery(connection, SqlDeleteCrestData, command =>
            {
                AddParameter(command, "character_common_id", characterCommonId);
                AddParameter(command, "item_uid", itemUId);
                AddParameter(command, "slot", slot);
            }) == 1;
        }

        public List<Crest> GetCrests(uint characterCommonId, string itemUId)
        {
            using TCon connection = OpenNewConnection();
            return GetCrests(connection, characterCommonId, itemUId);
        }

        public List<Crest> GetCrests(TCon connection, uint characterCommonId, string itemUId)
        {
            List<Crest> results = new List<Crest>();

            ExecuteInTransaction(connection =>
            {
                ExecuteReader(connection, SqlSelectAllCrestData,
                    command => {
                        AddParameter(command, "character_common_id", characterCommonId);
                        AddParameter(command, "item_uid", itemUId);
                    }, reader => {
                        while (reader.Read())
                        {
                            var result = ReadCrestData(reader);
                            results.Add(result);
                        }
                    });
            });

            return results;
        }
        private Crest ReadCrestData(TReader reader)
        {
            Crest obj = new Crest();
            obj.Slot = GetUInt32(reader, "slot");
            obj.CrestId = GetUInt32(reader, "crest_id");
            obj.Amount = GetUInt32(reader, "crest_amount");
            return obj;
        }

    }
}
