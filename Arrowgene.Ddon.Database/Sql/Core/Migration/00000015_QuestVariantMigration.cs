using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class QuestVariationMigration : IMigrationStrategy
    {
        public uint From => 14;
        public uint To => 15;

        private readonly DatabaseSetting DatabaseSetting;

        public QuestVariationMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_quest_variant.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
