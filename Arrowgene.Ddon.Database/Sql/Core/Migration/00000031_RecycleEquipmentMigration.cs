using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class RecycleEquipmentMigration : IMigrationStrategy
    {
        public uint From => 30;
        public uint To => 31;

        private readonly DatabaseSetting DatabaseSetting;

        public RecycleEquipmentMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_recycle_equipment.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
