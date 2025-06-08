using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class Msq30Migration(DatabaseSetting databaseSetting) : IMigrationStrategy
    {
        public uint From => 45;
        public uint To => 46;

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(databaseSetting, "Script/migration_msq_3.0.sql");
            db.Execute(conn, adaptedSchema, true);
            return true;
        }
    }
}
