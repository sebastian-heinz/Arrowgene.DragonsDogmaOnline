using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class EquipPresetMigration : IMigrationStrategy
    {
        public uint From => 21;
        public uint To => 22;

        private readonly DatabaseSetting DatabaseSetting;

        public EquipPresetMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_equipment_preset.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}

