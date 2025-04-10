using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class AchievementMigration : IMigrationStrategy
    {
        public uint From => 34;
        public uint To => 35;

        private readonly DatabaseSetting DatabaseSetting;

        public AchievementMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_achievements.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
