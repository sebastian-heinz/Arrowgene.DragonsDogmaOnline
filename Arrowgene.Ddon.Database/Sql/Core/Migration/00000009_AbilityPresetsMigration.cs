using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class AbilityPresetsMigration : IMigrationStrategy
    {
        public uint From => 8;
        public uint To => 9;

        private readonly DatabaseSetting DatabaseSetting;

        public AbilityPresetsMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/abilitypresets_migration_sqlite.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
