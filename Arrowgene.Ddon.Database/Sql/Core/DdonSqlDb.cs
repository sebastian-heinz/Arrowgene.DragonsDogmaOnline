using System;
using System.Data.Common;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    /// <summary>
    /// Implementation of Ddon database operations.
    /// </summary>
    public abstract partial class DdonSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonSqlDb<TCon, TCom>));

        public DdonSqlDb()
        {
        }

        protected override void Exception(Exception ex)
        {
            Logger.Exception(ex);
        }
    }
}
