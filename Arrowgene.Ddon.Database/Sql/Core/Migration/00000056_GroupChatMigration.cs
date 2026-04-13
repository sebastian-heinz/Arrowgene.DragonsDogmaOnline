using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class GroupChatMigration(DatabaseSetting databaseSetting) : IMigrationStrategy
    {
        public uint From => 55;
        public uint To => 56;

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(databaseSetting, "Script/group_chat_migration.sql");
            db.Execute(conn, adaptedSchema, true);
            return true;
        }
    }
}
