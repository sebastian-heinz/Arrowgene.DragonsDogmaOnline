using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class ItemSafetySettingMigration : IMigrationStrategy
    {
        public uint From => 21;
        public uint To => 22;

        private readonly DatabaseSetting DatabaseSetting;

        public ItemSafetySettingMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/item_safety_setting_migration.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
