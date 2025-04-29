using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class Msq21Migration : IMigrationStrategy
    {
        public uint From => 16;
        public uint To => 17;

        private readonly DatabaseSetting DatabaseSetting;

        public Msq21Migration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_msq_2.1.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
