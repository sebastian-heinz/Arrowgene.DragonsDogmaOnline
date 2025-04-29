using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class QuestVarientRefactorMigration : IMigrationStrategy
    {
        public uint From => 20;
        public uint To => 21;

        private readonly DatabaseSetting DatabaseSetting;

        public QuestVarientRefactorMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_quest_variant_refactor.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
