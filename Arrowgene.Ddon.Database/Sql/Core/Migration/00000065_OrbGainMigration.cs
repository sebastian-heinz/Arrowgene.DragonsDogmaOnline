using System.Data.Common;
using System.IO;
using System.Text;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class OrbGainMigration(DatabaseSetting databaseSetting) : IMigrationStrategy
    {
        public uint From => 64;
        public uint To => 65;

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(databaseSetting, "Script/orb_gain_migration.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
