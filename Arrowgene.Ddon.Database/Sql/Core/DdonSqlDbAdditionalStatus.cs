using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        protected static readonly string[] AdditionalStatusFields = new string[]
        {
            "item_uid", "character_id", "is_add_stat1", "is_add_stat2", "additional_status1", "additional_status2"
        };

        protected virtual string SqlInsertOrIgnoreADDS { get; } =
            $"INSERT INTO \"ddon_additional_status\" ({BuildQueryField(AdditionalStatusFields)}) " +
            $"SELECT {BuildQueryInsert(AdditionalStatusFields)} " +
            $"WHERE NOT EXISTS (SELECT 1 FROM \"ddon_additional_status\" WHERE \"item_uid\"=@item_uid);";

        private static readonly string SqlSelectADDS = $"SELECT {BuildQueryField(AdditionalStatusFields)} FROM \"ddon_additional_status\" WHERE \"item_uid\"=@item_uid;";

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
            return ExecuteNonQuery(conn, 
                $"INSERT INTO \"ddon_additional_status\" ({BuildQueryField(AdditionalStatusFields)}) " +
                $"VALUES (@item_uid, @character_id, @is_add_stat1, @is_add_stat2, @additional_status1, @additional_status2);",
                command =>
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

        private void AddParameter(TCom command, string parameterName, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;
            command.Parameters.Add(parameter);
        }
    }
}
