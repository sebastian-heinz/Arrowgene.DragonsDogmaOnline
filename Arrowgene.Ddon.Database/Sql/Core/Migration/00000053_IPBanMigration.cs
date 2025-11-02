using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class IPBanMigration(DatabaseSetting databaseSetting) : IMigrationStrategy
    {
        public uint From => 52;
        public uint To => 53;

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(databaseSetting, "Script/ip_bans_migration.sql");
            db.Execute(conn, adaptedSchema, true);
            return true;
        }
    }
}
