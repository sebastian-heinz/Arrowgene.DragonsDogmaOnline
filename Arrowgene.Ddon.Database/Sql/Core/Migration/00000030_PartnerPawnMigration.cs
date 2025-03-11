using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class PartnerPawnMigration : IMigrationStrategy
    {
        public uint From => 29;
        public uint To => 30;

        private readonly DatabaseSetting DatabaseSetting;

        public PartnerPawnMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_partner_pawn.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
