using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class DispelSealMigration(DatabaseSetting databaseSetting) : IMigrationStrategy
    {
        public uint From => 51;
        public uint To => 52;

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(databaseSetting, "Script/migration_dispel_seals.sql");
            db.Execute(conn, adaptedSchema, true);
            return true;
        }
    }
}
