using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class JobEmblemMigration : IMigrationStrategy
    {
        public uint From => 41;
        public uint To => 42;

        private readonly DatabaseSetting DatabaseSetting;

        public JobEmblemMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_job_emblem.sql");
            db.Execute(conn, adaptedSchema, true);
            return true;
        }

    }
}
