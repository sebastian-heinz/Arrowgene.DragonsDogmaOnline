using System.Data.Common;

namespace Arrowgene.Ddon.Database.Deferred
{
    public abstract class DeferredOperation
    {
        protected DeferredOperation()
        {
        }

        public abstract bool Handle(DbConnection conn);
    }

}
