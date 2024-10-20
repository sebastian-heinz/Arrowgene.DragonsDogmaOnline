using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class Msq20Migration : IMigrationStrategy
    {
        public uint From => 15;
        public uint To => 16;

        private readonly DatabaseSetting DatabaseSetting;

        public Msq20Migration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_msq_2.0.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
