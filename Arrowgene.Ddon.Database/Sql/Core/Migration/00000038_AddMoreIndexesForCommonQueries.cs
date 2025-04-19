using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class AddMoreIndexesForCommonQueries(DatabaseSetting databaseSetting) : IMigrationStrategy
    {
        public uint From => 37;
        public uint To => 38;

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(databaseSetting, "Script/add_more_indexes_for_common_queries.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
