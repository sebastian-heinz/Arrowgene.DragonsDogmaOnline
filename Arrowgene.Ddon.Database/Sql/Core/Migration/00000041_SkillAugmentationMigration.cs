using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class SkillAugmentationMigration : IMigrationStrategy
    {
        public uint From => 40;
        public uint To => 41;

        private readonly DatabaseSetting DatabaseSetting;

        public SkillAugmentationMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_skill_augmentation.sql");
            db.Execute(conn, adaptedSchema, true);
            return true;
        }

    }
}
