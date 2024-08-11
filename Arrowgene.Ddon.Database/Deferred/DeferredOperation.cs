using System.Data.Common;

namespace Arrowgene.Ddon.Database.Deferred
{
    public abstract class DeferredOperation
    {
        protected DeferredOperation(IDatabase db)
        {
            Database = db;
        }

        protected IDatabase Database;

        public abstract bool Handle(DbConnection conn);
    }

}
