using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class JobStatFixMigration : IMigrationStrategy
    {
        public uint From => 31;
        public uint To => 32;

        private readonly DatabaseSetting DatabaseSetting;

        public JobStatFixMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_job_stat_fix.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
