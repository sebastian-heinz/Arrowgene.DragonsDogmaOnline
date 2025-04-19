#nullable enable

using System;
using System.Data.Common;
using System.Text;
using Arrowgene.Logging;
using MySqlConnector;

namespace Arrowgene.Ddon.Database.Sql.Core;

/// <summary>
///     Implementation of Ddon database operations.
/// </summary>
public partial class DdonSqlDb : SqlDb
{
    private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonSqlDb));

    public static string BuildQueryField(params string[][] fieldLists)
    {
        return BuildQueryField(null, fieldLists);
    }

    public static string BuildQueryField(string table, params string[][] fieldLists)
    {
        StringBuilder sb = new();
        for (int i = 0; i < fieldLists.Length; i++)
        {
            string[] fieldList = fieldLists[i];
            for (int j = 0; j < fieldList.Length; j++)
            {
                string field = fieldList[j];
                if (table != null)
                {
                    sb.Append('\"');
                    sb.Append(table);
                    sb.Append("\".");
                }

                sb.Append('\"');
                sb.Append(field);
                sb.Append('\"');
                if (j < fieldList.Length - 1) sb.Append(", ");
            }
        }

        return sb.ToString();
    }

    public static string BuildQueryUpdate(params string[][] fieldLists)
    {
        return BuildQueryUpdateWithPrefix("@", fieldLists);
    }

    protected static string BuildQueryUpdateWithPrefix(string prefix, params string[][] fieldLists)
    {
        StringBuilder sb = new();
        for (int i = 0; i < fieldLists.Length; i++)
        {
            string[] fieldList = fieldLists[i];
            for (int j = 0; j < fieldList.Length; j++)
            {
                string field = fieldList[j];
                sb.Append($"\"{field}\"={prefix}{field}");
                if (j < fieldList.Length - 1) sb.Append(", ");
            }

            if (i < fieldLists.Length - 1) sb.Append(", ");
        }

        return sb.ToString();
    }

    public static string BuildQueryInsert(params string[][] fieldLists)
    {
        StringBuilder sb = new();
        for (int i = 0; i < fieldLists.Length; i++)
        {
            string[] fieldList = fieldLists[i];
            sb.Append('@');
            sb.Append(string.Join(", @", fieldList));
            if (i < fieldLists.Length - 1) sb.Append(", ");
        }

        return sb.ToString();
    }

    public override T ExecuteQuerySafe<T>(DbConnection? connectionIn, Func<DbConnection, T> work)
    {
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            return work.Invoke(connection);
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }
    }

    public override void ExecuteQuerySafe(DbConnection? connectionIn, Action<DbConnection> work)
    {
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            work.Invoke(connection);
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }
    }

    protected override void Exception(Exception ex, string? query = null)
    {
        Logger.Exception(ex);
        if (query != null) Logger.Error($"Exception during query: {query}");
    }

    protected virtual long AutoIncrement(MySqlConnection connection, MySqlCommand command)
    {
        throw new NotImplementedException();
    }
}
