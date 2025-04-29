using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class EpitaphRoadMigration : IMigrationStrategy
    {
        public uint From => 24;
        public uint To => 25;

        private readonly DatabaseSetting DatabaseSetting;

        public EpitaphRoadMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_epitaph_road.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
