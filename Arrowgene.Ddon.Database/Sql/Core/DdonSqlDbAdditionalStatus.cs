using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        #region Constants

        protected static readonly string[] AdditionalStatusFields = new string[]
        {
            "item_uid", "character_id", "is_add_stat1", "is_add_stat2", "additional_status1", "additional_status2"
        };

        protected virtual string SqlInsertOrIgnoreADDS { get; } =
            $"INSERT INTO \"ddon_additional_status\" ({BuildQueryField(AdditionalStatusFields)}) " +
            $"SELECT {BuildQueryInsert(AdditionalStatusFields)} " +
            $"WHERE NOT EXISTS (SELECT 1 FROM \"ddon_additional_status\" WHERE \"item_uid\"=@item_uid);";

        protected virtual string SqlUpdateADDS { get; } =
            $"UPDATE \"ddon_additional_status\" SET " +
            $"\"character_id\"=@character_id, \"is_add_stat1\"=@is_add_stat1, \"is_add_stat2\"=@is_add_stat2, " +
            $"\"additional_status1\"=@additional_status1, \"additional_status2\"=@additional_status2 " +
            $"WHERE \"item_uid\"=@item_uid;";

        private static readonly string SqlSelectADDS =
            $"SELECT {BuildQueryField(AdditionalStatusFields)} FROM \"ddon_additional_status\" WHERE \"item_uid\"=@item_uid;";

        protected virtual string SqlInsertAddStatus { get; } =
            $"INSERT INTO \"ddon_additional_status\" ({BuildQueryField(AdditionalStatusFields)}) " +
            $"VALUES (@item_uid, @character_id, @is_add_stat1, @is_add_stat2, @additional_status1, @additional_status2);";

        #endregion

        #region Methods

        public bool InsertIfNotExistsAddStatus(TCon conn, string itemUid, uint characterId, byte isAddStat1, byte isAddStat2, ushort addStat1, ushort addStat2)
        {
            return ExecuteNonQuery(conn, SqlInsertOrIgnoreADDS, command =>
            {
                AddParameter(command, "item_uid", itemUid);
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "is_add_stat1", isAddStat1);
                AddParameter(command, "is_add_stat2", isAddStat2);
                AddParameter(command, "additional_status1", addStat1);
                AddParameter(command, "additional_status2", addStat2);
            }) == 1;
        }

        public bool InsertIfNotExistsAddStatus(string itemUid, uint characterId, byte isAddStat1, byte isAddStat2, ushort addStat1, ushort addStat2)
        {
            using TCon connection = OpenNewConnection();
            return InsertIfNotExistsAddStatus(connection, itemUid, characterId, isAddStat1, isAddStat2, addStat1, addStat2);
        }

        public bool InsertAddStatus(TCon conn, string itemUid, uint characterId, byte isAddStat1, byte isAddStat2, ushort addStat1, ushort addStat2)
        {
            return ExecuteNonQuery(conn, SqlInsertAddStatus, command =>
            {
                AddParameter(command, "item_uid", itemUid);
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "is_add_stat1", isAddStat1);
                AddParameter(command, "is_add_stat2", isAddStat2);
                AddParameter(command, "additional_status1", addStat1);
                AddParameter(command, "additional_status2", addStat2);
            }) == 1;
        }

        public bool InsertAddStatus(string itemUid, uint characterId, byte isAddStat1, byte isAddStat2, ushort addStat1, ushort addStat2)
        {
            using TCon connection = OpenNewConnection();
            return InsertAddStatus(connection, itemUid, characterId, isAddStat1, isAddStat2, addStat1, addStat2);
        }

        public bool UpdateAddStatus(TCon conn, string itemUid, uint characterId, byte isAddStat1, byte isAddStat2, ushort addStat1, ushort addStat2)
        {
            return ExecuteNonQuery(conn, SqlUpdateADDS, command =>
            {
                AddParameter(command, "item_uid", itemUid);
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "is_add_stat1", isAddStat1);
                AddParameter(command, "is_add_stat2", isAddStat2);
                AddParameter(command, "additional_status1", addStat1);
                AddParameter(command, "additional_status2", addStat2);
            }) == 1;
        }

        public bool UpdateAddStatus(string itemUid, uint characterId, byte isAddStat1, byte isAddStat2, ushort addStat1, ushort addStat2)
        {
            using TCon connection = OpenNewConnection();
            return UpdateAddStatus(connection, itemUid, characterId, isAddStat1, isAddStat2, addStat1, addStat2);
        }

        public List<CDataAddStatusParam> GetAddStatusByUID(string itemUid)
        {
            using TCon connection = OpenNewConnection();
            return GetAddStatusByUID(connection, itemUid);
        }
        public List<CDataAddStatusParam> GetAddStatusByUID(TCon connection, string itemUid)
        {
            List<CDataAddStatusParam> results = new List<CDataAddStatusParam>();

            ExecuteInTransaction(connection =>
            {
                ExecuteReader(connection, SqlSelectADDS,
                    command => {
                        AddParameter(command, "@item_uid", itemUid);
                    }, reader => {
                        while (reader.Read())
                        {
                            var result = ReadAddStatus(reader);
                            results.Add(result);
                        }
                    });
            });

            return results;
        }

        private CDataAddStatusParam ReadAddStatus(TReader reader)
        {
            CDataAddStatusParam AddStatus = new CDataAddStatusParam();
            AddStatus.IsAddStat1 = GetBoolean(reader, "is_add_stat1");
            AddStatus.IsAddStat2 = GetBoolean(reader, "is_add_stat2");
            AddStatus.AdditionalStatus1 = GetUInt16(reader, "additional_status1");
            AddStatus.AdditionalStatus2 = GetUInt16(reader, "additional_status2");
            return AddStatus;
        }

        #endregion
    }
}
