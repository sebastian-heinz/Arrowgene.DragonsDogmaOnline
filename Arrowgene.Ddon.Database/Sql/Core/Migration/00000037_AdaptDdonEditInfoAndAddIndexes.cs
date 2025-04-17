using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class FixDdonEditInfoAndAddIndexes : IMigrationStrategy
    {
        public uint From => 36;
        public uint To => 37;

        private readonly DatabaseSetting DatabaseSetting;

        public FixDdonEditInfoAndAddIndexes(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/adapt_ddon_edit_info_and_add_indexes.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
