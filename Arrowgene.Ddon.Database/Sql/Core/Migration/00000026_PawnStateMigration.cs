using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class PawnStateMigration : IMigrationStrategy
    {
        public uint From => 25;
        public uint To => 26;

        private readonly DatabaseSetting DatabaseSetting;

        public PawnStateMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_pawn_state.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
