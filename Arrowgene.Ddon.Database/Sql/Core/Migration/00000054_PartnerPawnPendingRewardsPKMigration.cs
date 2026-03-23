using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class PartnerPawnPendingRewardsPKMigration(DatabaseSetting databaseSetting) : IMigrationStrategy
    {
        public uint From => 53;
        public uint To => 54;

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(databaseSetting, "Script/migration_partner_pawn_pending_rewards_pk.sql");
            db.Execute(conn, adaptedSchema, true);
            return true;
        }
    }
}