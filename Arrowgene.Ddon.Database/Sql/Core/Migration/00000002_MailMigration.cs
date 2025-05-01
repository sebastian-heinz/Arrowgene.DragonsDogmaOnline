using System;
using System.Data.Common;
using System.IO;
using System.Text;
using Arrowgene.Ddon.Database.Model;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class MailMigration : IMigrationStrategy
    {
        public uint From => 1;
        public uint To => 2;

        private readonly DatabaseSetting DatabaseSetting;

        public MailMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string schemaFilePath = Path.Combine(DatabaseSetting.DatabaseFolder, "Script/migration_mail_sqlite.sql");
            string schema = File.ReadAllText(schemaFilePath, Encoding.UTF8);
            string adaptedSchema = DdonDatabaseBuilder.AdaptSQLiteSchemaTo(DatabaseSetting.Type, schema);
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
