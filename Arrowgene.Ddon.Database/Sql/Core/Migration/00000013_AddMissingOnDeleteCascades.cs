using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class AddMissingOnDeleteCascades : IMigrationStrategy
    {
        public uint From => 12;
        public uint To => 13;

        private readonly DatabaseSetting DatabaseSetting;

        public AddMissingOnDeleteCascades(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/AddMissingOnDeleteCascades_migration.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
