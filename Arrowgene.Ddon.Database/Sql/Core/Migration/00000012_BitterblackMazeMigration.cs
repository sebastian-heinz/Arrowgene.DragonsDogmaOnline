using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class BitterblackMazeMigration : IMigrationStrategy
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(BitterblackMazeMigration));

        public uint From => 11;
        public uint To => 12;

        private readonly DatabaseSetting DatabaseSetting;

        public BitterblackMazeMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/bitterblack_maze_migration.sql");
            db.Execute(conn, adaptedSchema);
            return true;
        }
    }
}
