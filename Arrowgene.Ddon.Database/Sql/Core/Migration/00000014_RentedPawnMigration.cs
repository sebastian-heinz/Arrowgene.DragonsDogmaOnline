using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class RentedPawnMigrationA : IMigrationStrategy
    {
        public uint From => 13;
        public uint To => 14;

        private readonly DatabaseSetting DatabaseSetting;

        public RentedPawnMigrationA(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_rented_pawn.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
