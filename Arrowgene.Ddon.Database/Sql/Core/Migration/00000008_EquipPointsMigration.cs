using System;
using System.Data.Common;
using System.IO;
using System.Text;
using Arrowgene.Ddon.Database.Model;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class AddEquipPointsMigration : IMigrationStrategy
    {
        public uint From => 7;
        public uint To => 8;

        private readonly DatabaseSetting DatabaseSetting;

        public AddEquipPointsMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/equippoints_migration.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
