using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        protected static readonly string[] MetaFields = new string[]
        {
            "db_version"
        };

        private static readonly string SqlInsertOrIgnoreMeta = $"INSERT INTO \"meta\" ({BuildQueryField(MetaFields)}) SELECT ({BuildQueryInsert(MetaFields)}) WHERE NOT EXISTS (SELECT 1 FROM \"meta\" LIMIT 1);";
        private static readonly string SqlUpdateMeta = $"UPDATE \"meta\" SET {BuildQueryUpdate(MetaFields)};";
        private static readonly string SqlSelectMeta = $"SELECT {BuildQueryField(MetaFields)} FROM \"meta\" LIMIT 1;";
        

        public bool CreateMeta(DatabaseMeta meta)
        {
            int rowsAffected = ExecuteNonQuery(SqlInsertOrIgnoreMeta, command =>
            {
                AddParameter(command, "@db_version", meta.DatabaseVersion);
            });

            return rowsAffected > NoRowsAffected;
        }

        public bool SetMeta(DatabaseMeta meta)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateMeta, command =>
            {
                AddParameter(command, "@db_version", meta.DatabaseVersion);
            });

            return rowsAffected > NoRowsAffected;
        }

        public DatabaseMeta GetMeta()
        {
            DatabaseMeta meta = new DatabaseMeta();
            ExecuteReader(SqlSelectMeta, command => {}, reader =>
            {
                if(reader.Read())
                {
                    meta.DatabaseVersion = GetUInt32(reader, "db_version");
                }
            });
            return meta;
        }
    }
}
