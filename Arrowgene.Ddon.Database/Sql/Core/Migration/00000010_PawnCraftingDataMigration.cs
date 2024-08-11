using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class PawnCraftingDataMigration : IMigrationStrategy
    {
        public uint From => 9;
        public uint To => 10;

        private readonly DatabaseSetting DatabaseSetting;

        public PawnCraftingDataMigration(DatabaseSetting databaseSetting)
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
