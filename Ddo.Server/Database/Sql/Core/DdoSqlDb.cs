using System;
using System.Data.Common;
using Arrowgene.Services.Logging;
using Ddo.Server.Logging;

namespace Ddo.Server.Database.Sql.Core
{
    /// <summary>
    /// Implementation of Ddo database operations.
    /// </summary>
    public abstract partial class DdoSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        protected readonly DdoLogger Logger;


        public DdoSqlDb()
        {
            Logger = LogProvider.Logger<DdoLogger>(this);
        }

        protected override void Exception(Exception ex)
        {
            Logger.Exception(ex);
        }

    }
}
