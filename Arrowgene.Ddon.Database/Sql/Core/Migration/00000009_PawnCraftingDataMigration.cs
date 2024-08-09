using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class MyPawnCraftingDataMigration : IMigrationStrategy
    {
        public uint From => 8;
        public uint To => 9;

        private readonly DatabaseSetting DatabaseSetting;

        public MyPawnCraftingDataMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/pawncraftingdata_migration.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
