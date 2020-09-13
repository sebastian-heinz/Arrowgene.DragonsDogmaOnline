using System;
using System.Data.Common;
using Arrowgene.Logging;

namespace Arrowgene.Ddo.Database.Sql.Core
{
    /// <summary>
    /// Implementation of Ddo database operations.
    /// </summary>
    public abstract partial class DdoSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdoSqlDb<TCon, TCom>));

        public DdoSqlDb()
        {
        }

        protected override void Exception(Exception ex)
        {
            Logger.Exception(ex);
        }
    }
}
