using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class CharacterTrackingMigration(DatabaseSetting databaseSetting) : IMigrationStrategy
    {
        public uint From => 58;
        public uint To => 59;

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(databaseSetting, "Script/migration_connection_characterid.sql");
            db.Execute(conn, adaptedSchema, true);
            return true;
        }
    }
}
