using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class RankRecordMigration : IMigrationStrategy
    {
        public uint From => 28;
        public uint To => 29;

        private readonly DatabaseSetting DatabaseSetting;

        public RankRecordMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_rank_records.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
