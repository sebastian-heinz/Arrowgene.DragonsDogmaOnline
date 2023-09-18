using System;
using System.Data.Common;
using System.Text;
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

        public static string BuildQueryField(params string[][] fieldLists)
        {
            return BuildQueryField(null, fieldLists);
        }

        public static string BuildQueryField(string table, params string[][] fieldLists)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < fieldLists.Length; i++)
            {
                string[] fieldList = fieldLists[i];
                for (int j = 0; j < fieldList.Length; j++)
                {
                    string field = fieldList[j];
                    if(table != null)
                    {
                        sb.Append('\"');
                        sb.Append(table);
                        sb.Append("\".");
                    }
                    sb.Append('\"');
                    sb.Append(field);
                    sb.Append('\"');
                    if (j < fieldList.Length - 1)
                    {
                        sb.Append(", ");
                    }
                }
            }

            return sb.ToString();
        }

        public static string BuildQueryUpdate(params string[][] fieldLists)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < fieldLists.Length; i++)
            {
                string[] fieldList = fieldLists[i];
                for (int j = 0; j < fieldList.Length; j++)
                {
                    string field = fieldList[j];
                    sb.Append($"\"{field}\"=@{field}");
                    if (j < fieldList.Length - 1)
                    {
                        sb.Append(", ");
                    }
                }

                if (i < fieldLists.Length - 1)
                {
                    sb.Append(", ");
                }
            }

            return sb.ToString();
        }

        protected static string BuildQueryUpdateWithTempTable(string tempName, params string[][] fieldLists)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < fieldLists.Length; i++)
            {
                string[] fieldList = fieldLists[i];
                for (int j = 0; j < fieldList.Length; j++)
                {
                    string field = fieldList[j];
                    sb.Append($"\"{field}\"={tempName}.{field}");
                    if (j < fieldList.Length - 1)
                    {
                        sb.Append(", ");
                    }
                }

                if (i < fieldLists.Length - 1)
                {
                    sb.Append(", ");
                }
            }

            return sb.ToString();
        }

        public static string BuildQueryInsert(params string[][] fieldLists)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < fieldLists.Length; i++)
            {
                string[] fieldList = fieldLists[i];
                sb.Append('@');
                sb.Append(string.Join(", @", fieldList));
                if (i < fieldLists.Length - 1)
                {
                    sb.Append(", ");
                }
            }

            return sb.ToString();
        }

        protected override void Exception(Exception ex, string query)
        {
            Logger.Exception(ex);
            if (query != null)
            {
                Logger.Error($"Exception during query: {query}");
            }
        }
    }
}
