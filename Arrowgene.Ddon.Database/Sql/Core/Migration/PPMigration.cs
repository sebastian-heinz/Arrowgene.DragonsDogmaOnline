using Arrowgene.Ddon.Database.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class PPMigration : IMigrationStrategy
    {
        public uint From => 2; 
        public uint To => 3; //TODO: Watch for this when merging.

        private readonly DatabaseSetting DatabaseSetting;

        public PPMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string schemaFilePath = Path.Combine(DatabaseSetting.DatabaseFolder, "Script/migration_pp_sqlite.sql");
            string schema = File.ReadAllText(schemaFilePath, Encoding.UTF8);
            DatabaseType databaseType = (DatabaseType)Enum.Parse(typeof(DatabaseType), DatabaseSetting.Type, true);
            string adaptedSchema = DdonDatabaseBuilder.AdaptSQLiteSchemaTo(databaseType, schema);
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
