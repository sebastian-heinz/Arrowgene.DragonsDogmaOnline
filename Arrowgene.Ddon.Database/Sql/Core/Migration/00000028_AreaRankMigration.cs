using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class AreaRankMigration : IMigrationStrategy
    {
        public uint From => 27;
        public uint To => 28;

        private readonly DatabaseSetting DatabaseSetting;

        public AreaRankMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_arearank.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
