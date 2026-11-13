using System.Data.Common;
using System.IO;
using System.Text;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class PawnShareRangeMigration(DatabaseSetting databaseSetting) : IMigrationStrategy
    {
        public uint From => 67;
        public uint To => 68;

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(databaseSetting, "Script/pawn_share_range_migration.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
