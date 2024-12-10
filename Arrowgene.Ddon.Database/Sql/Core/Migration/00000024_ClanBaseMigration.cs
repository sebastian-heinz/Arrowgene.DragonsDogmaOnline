using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class ClanBaseMigration : IMigrationStrategy
    {
        public uint From => 23;
        public uint To => 24;

        private readonly DatabaseSetting DatabaseSetting;

        public ClanBaseMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/clan_base_migration.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
