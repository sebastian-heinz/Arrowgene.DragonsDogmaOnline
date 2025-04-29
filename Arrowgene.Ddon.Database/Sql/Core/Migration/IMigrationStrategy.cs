using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public interface IMigrationStrategy
    {
        uint From { get; }
        uint To { get; }
        bool DisableTransaction => false;
        bool Migrate(IDatabase db, DbConnection conn);
    }
}
