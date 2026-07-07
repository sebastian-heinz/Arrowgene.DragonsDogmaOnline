using System.Data.Common;
using System.IO;
using System.Text;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class OrbGainMigration : IMigrationStrategy
    {
        public uint From => 64;
        public uint To => 65;

        private readonly DatabaseSetting DatabaseSetting;

        public OrbGainMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string scriptPath = Path.Combine(DatabaseSetting.DatabaseFolder, "Script/orb_gain_migration.sql");
            string script = File.ReadAllText(scriptPath, Encoding.UTF8);
            string adaptedScript = DdonDatabaseBuilder.AdaptSQLiteSchemaTo(DatabaseSetting.Type, script);
            db.Execute(conn, adaptedScript);
            return true;
        }
    }
}
