using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    class LightQuestMigration : IMigrationStrategy
    {
        public uint From => 22;
        public uint To => 23;

        private readonly DatabaseSetting DatabaseSetting;

        public LightQuestMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_light_quest_type.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
