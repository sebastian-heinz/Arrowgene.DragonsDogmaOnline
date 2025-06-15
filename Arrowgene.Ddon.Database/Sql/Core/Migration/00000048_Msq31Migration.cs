using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class Msq31Migration(DatabaseSetting databaseSetting) : IMigrationStrategy
    {
        public uint From => 47;
        public uint To => 48;

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(databaseSetting, "Script/migration_msq_3.1.sql");
            db.Execute(conn, adaptedSchema, true);
            return true;
        }
    }
}
