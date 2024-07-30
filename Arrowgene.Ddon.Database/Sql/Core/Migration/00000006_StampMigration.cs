using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class StampMigration : IMigrationStrategy
    {
        public uint From => 5;
        public uint To => 6;

        private readonly DatabaseSetting DatabaseSetting;

        public StampMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_stamp_sqlite.sql");
            db.Execute(conn, adaptedSchema);

            return true;
        }
    }
}
