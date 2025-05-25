using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class DdonStampBonusMinimumDateMigration(DatabaseSetting databaseSetting) : IMigrationStrategy
    {
        public uint From => 42;
        public uint To => 43;

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(databaseSetting, "Script/ddon_stamp_bonus_minimum_date_migration.sql");
            db.Execute(conn, adaptedSchema, true);
            return true;
        }
    }
}
