using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class Msq22Migration : IMigrationStrategy
    {
        public uint From => 17;
        public uint To => 18;

        private readonly DatabaseSetting DatabaseSetting;

        public Msq22Migration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_msq_2.2.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
