using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class JobItemStorageSlotMigration : IMigrationStrategy
    {
        public uint From => 32;
        public uint To => 33;

        private readonly DatabaseSetting DatabaseSetting;

        public JobItemStorageSlotMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_job_item_storage_slot.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}