using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class FixDdonEditInfoSchema : IMigrationStrategy
    {
        public uint From => 35;
        public uint To => 36;

        private readonly DatabaseSetting DatabaseSetting;

        public FixDdonEditInfoSchema(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/DdonEditInfoSmallintToInteger_migration.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
