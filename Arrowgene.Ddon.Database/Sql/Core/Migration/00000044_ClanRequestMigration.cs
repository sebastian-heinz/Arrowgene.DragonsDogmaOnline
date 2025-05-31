using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class ClanRequestMigration : IMigrationStrategy
    {
        public uint From => 43;
        public uint To => 44;

        private readonly DatabaseSetting DatabaseSetting;

        public ClanRequestMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_clan_requests.sql");
            db.Execute(conn, adaptedSchema, true);
            return true;
        }
    }
}
