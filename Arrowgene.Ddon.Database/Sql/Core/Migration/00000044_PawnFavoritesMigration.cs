using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class PawnFavoritesMigration(DatabaseSetting databaseSetting) : IMigrationStrategy
    {
        public uint From => 43;
        public uint To => 44;

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(databaseSetting, "Script/migration_pawn_favorites.sql");
            db.Execute(conn, adaptedSchema, true);
            return true;
        }
    }
}
