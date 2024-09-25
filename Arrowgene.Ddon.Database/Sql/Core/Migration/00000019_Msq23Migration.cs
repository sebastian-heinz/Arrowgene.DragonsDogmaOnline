using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class Msq23Migration : IMigrationStrategy
    {
        public uint From => 18;
        public uint To => 19;

        private readonly DatabaseSetting DatabaseSetting;

        public Msq23Migration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_msq_2.3.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
