using System;
using System.Data.Common;
using System.IO;
using System.Text;
using Arrowgene.Ddon.Database.Model;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class AddEquipPointsMigration : IMigrationStrategy
    {
        public uint From => 7;  // Update the "From" version if necessary
        public uint To => 8;    // Set the "To" version for this migration

        private readonly DatabaseSetting DatabaseSetting;

        public AddEquipPointsMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string schemaFilePath = Path.Combine(DatabaseSetting.DatabaseFolder, "Script/equippoints_migration.sql");
            string schema = File.ReadAllText(schemaFilePath, Encoding.UTF8);

            string adaptedSchema = DdonDatabaseBuilder.AdaptSQLiteSchemaTo(DatabaseSetting.Type, schema);
            
            // Execute the schema changes
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
