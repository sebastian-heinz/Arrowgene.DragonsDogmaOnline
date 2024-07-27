using Arrowgene.Logging;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class EquipPlayPointMigration : IMigrationStrategy
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(EquipMigration));

        public uint From => 2;
        public uint To => 4;

        private readonly DatabaseSetting DatabaseSetting;

        public EquipPlayPointMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            var equipMigration = new EquipMigration(DatabaseSetting);
            var playPointMigration = new PPMigration(DatabaseSetting);

            return playPointMigration.Migrate(db, conn) && equipMigration.Migrate(db, conn);
        }
    }
}
