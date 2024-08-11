using System;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Deferred
{
    public class GenericDeferred : DeferredOperation
    {
        public GenericDeferred(IDatabase db, Func<IDatabase, DbConnection, bool> action) : base(db)
        {
            Action = action;
        }

        private readonly Func<IDatabase, DbConnection, bool> Action;

        public override bool Handle(DbConnection conn)
        {
            return Action(Database, conn);
        }
    }
}
