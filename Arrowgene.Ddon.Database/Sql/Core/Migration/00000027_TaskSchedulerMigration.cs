using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class TaskSchedulerMigration : IMigrationStrategy
    {
        public uint From => 26;
        public uint To => 27;

        private readonly DatabaseSetting DatabaseSetting;

        public TaskSchedulerMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_scheduling.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}

