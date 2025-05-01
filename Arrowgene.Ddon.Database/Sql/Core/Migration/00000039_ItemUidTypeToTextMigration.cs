using System;
using System.Data.Common;
using System.IO;
using System.Text;
using Arrowgene.Ddon.Database.Model;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class ItemUidTypeToTextMigration : IMigrationStrategy
    {
        public uint From => 38;
        public uint To => 39;
        public bool DisableTransaction => true;

        private readonly DatabaseSetting DatabaseSetting;

        public ItemUidTypeToTextMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }
        
        public bool Migrate(IDatabase db, DbConnection conn)
        {
            Enum.TryParse(DatabaseSetting.Type, true, out DatabaseType dbType);
            db.Execute(conn, File.ReadAllText(Path.Combine(DatabaseSetting.DatabaseFolder, $"Script/item_uid_type_to_text_migration_{dbType.ToString().ToLowerInvariant()}.sql"), Encoding.UTF8), true);
            return true;
        }
    }
}
