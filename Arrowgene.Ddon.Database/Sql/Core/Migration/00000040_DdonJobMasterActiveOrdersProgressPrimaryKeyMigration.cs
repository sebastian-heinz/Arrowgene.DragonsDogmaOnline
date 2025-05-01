using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class DdonJobMasterActiveOrdersProgressPrimaryKeyMigration : IMigrationStrategy
    {
        public uint From => 39;
        public uint To => 40;

        private readonly DatabaseSetting DatabaseSetting;

        public DdonJobMasterActiveOrdersProgressPrimaryKeyMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }
        
        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/ddon_job_master_active_orders_progress_primary_key_migration.sql");
            db.Execute(conn, adaptedSchema, true);
            return true;
        }

    }
}
