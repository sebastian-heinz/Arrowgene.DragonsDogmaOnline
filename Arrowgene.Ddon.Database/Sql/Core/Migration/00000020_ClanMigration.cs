using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class ClanMigration : IMigrationStrategy
    {
        public uint From => 19;
        public uint To => 20;

        private readonly DatabaseSetting DatabaseSetting;

        public ClanMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/clan_migration.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
