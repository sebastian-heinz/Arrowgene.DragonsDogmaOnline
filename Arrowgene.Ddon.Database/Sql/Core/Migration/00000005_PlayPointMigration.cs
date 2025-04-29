using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class PPMigration : IMigrationStrategy
    {
        public uint From => 4; 
        public uint To => 5;

        private readonly DatabaseSetting DatabaseSetting;

        public PPMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_pp_sqlite.sql");
            db.Execute(conn, adaptedSchema);

            return true;
        }
    }
}
