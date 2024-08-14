using System;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Deferred
{
    public class GenericDeferred : DeferredOperation
    {
        public GenericDeferred(Func<DbConnection, bool> action) : base()
        {
            Action = action;
        }

        private readonly Func<DbConnection, bool> Action;

        public override bool Handle(DbConnection conn)
        {
            return Action(conn);
        }
    }
}
