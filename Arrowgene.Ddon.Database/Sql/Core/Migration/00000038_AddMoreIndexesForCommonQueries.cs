using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class AddMoreIndexesForCommonQueries : IMigrationStrategy
    {
        public uint From => 37;
        public uint To => 38;

        private readonly DatabaseSetting DatabaseSetting;

        public AddMoreIndexesForCommonQueries(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }
        
        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/add_more_indexes_for_common_queries.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
