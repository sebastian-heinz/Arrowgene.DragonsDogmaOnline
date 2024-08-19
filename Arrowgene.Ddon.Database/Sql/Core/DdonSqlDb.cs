using Arrowgene.Ddon.Database.Sql.Core.Migration;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    /// <summary>
    /// Implementation of Ddon database operations.
    /// </summary>
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>, IDatabase
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonSqlDb<TCon, TCom, TReader>));

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
            return BuildQueryUpdateWithPrefix("@", fieldLists);
        }

        protected static string BuildQueryUpdateWithPrefix(string prefix, params string[][] fieldLists)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < fieldLists.Length; i++)
            {
                string[] fieldList = fieldLists[i];
                for (int j = 0; j < fieldList.Length; j++)
                {
                    string field = fieldList[j];
                    sb.Append($"\"{field}\"={prefix}{field}");
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
        
        public abstract bool CreateDatabase();

        public void Execute(DbConnection conn, string sql)
        {
            base.Execute((TCon) conn, sql);
        }

        public bool ExecuteInTransaction(Action<DbConnection> action)
        {
            return base.ExecuteInTransaction((conn) => action.Invoke(conn));
        }

        public int ExecuteNonQuery(DbConnection conn, string sql, Action<DbCommand> action)
        {
            return base.ExecuteNonQuery((TCon) conn, sql, action);
        }

        public void ExecuteReader(DbConnection conn, string sql, Action<DbDataReader> action)
        {
            base.ExecuteReader((TCon) conn, sql, (reader) => action.Invoke((TReader) reader));
        }

        public void ExecuteReader(DbConnection conn, string sql, Action<DbCommand> commandAction, Action<DbDataReader> readAction)
        {
            base.ExecuteReader((TCon) conn, sql, (command) => commandAction.Invoke((TCom) command), (reader) => readAction.Invoke((TReader) reader));
        }

        public bool MigrateDatabase(DatabaseMigrator migrator, uint toVersion)
        {
            uint currentVersion = GetMeta().DatabaseVersion;
            bool result = migrator.MigrateDatabase(this, currentVersion, toVersion);
            if (result)
            {
                SetMeta(new DatabaseMeta()
                {
                    DatabaseVersion = DdonDatabaseBuilder.Version
                });
            }
            return result;
        }

        protected override void Exception(Exception ex, string query)
        {
            Logger.Exception(ex);
            if (query != null)
            {
                Logger.Error($"Exception during query: {query}");
            }
        }

        public void AddParameter(DbCommand command, string name, object? value, DbType type)
        {
            base.AddParameter((TCom) command, name, value, type);
        }

        public void AddParameter(DbCommand command, string name, string value)
        {
            base.AddParameter((TCom) command, name, value);
        }

        public void AddParameter(DbCommand command, string name, Int32 value)
        {
            base.AddParameter((TCom)command, name, value);
        }

        public void AddParameter(DbCommand command, string name, float value)
        {
            base.AddParameter((TCom)command, name, value);
        }

        public void AddParameter(DbCommand command, string name, byte value)
        {
            base.AddParameter((TCom)command, name, value);
        }

        public void AddParameter(DbCommand command, string name, UInt16 value)
        {
            base.AddParameter((TCom)command, name, value);
        }

        public void AddParameter(DbCommand command, string name, UInt32 value)
        {
            base.AddParameter((TCom)command, name, value);
        }

        public void AddParameter(DbCommand command, string name, byte[] value)
        {
            base.AddParameter((TCom)command, name, value);
        }

        public void AddParameter(DbCommand command, string name, bool value)
        {
            base.AddParameter((TCom)command, name, value);
        }

        public string? GetStringNullable(DbDataReader reader, int ordinal)
        {
            return base.GetStringNullable((TReader) reader, ordinal);
        }

        public byte[]? GetBytesNullable(DbDataReader reader, int ordinal, int size)
        {
            return base.GetBytesNullable((TReader)reader, ordinal, size);
        }

        public int GetInt32(DbDataReader reader, string column)
        {
            return base.GetInt32((TReader)reader, column);
        }

        public uint GetUInt32(DbDataReader reader, string column)
        {
            return base.GetUInt32((TReader)reader, column);
        }

        public byte GetByte(DbDataReader reader, string column)
        {
            return base.GetByte((TReader)reader, column);
        }

        public short GetInt16(DbDataReader reader, string column)
        {
            return base.GetInt16((TReader)reader, column);
        }

        public ushort GetUInt16(DbDataReader reader, string column)
        {
            return base.GetUInt16((TReader)reader, column);
        }

        public long GetInt64(DbDataReader reader, string column)
        {
            return base.GetInt64((TReader)reader, column);
        }

        public ulong GetUInt64(DbDataReader reader, string column)
        {
            return base.GetUInt64((TReader)reader, column);
        }

        public float GetFloat(DbDataReader reader, string column)
        {
            return base.GetFloat((TReader)reader, column);
        }

        public string GetString(DbDataReader reader, string column)
        {
            return base.GetString((TReader)reader, column);
        }

        public bool GetBoolean(DbDataReader reader, string column)
        {
            return base.GetBoolean((TReader)reader, column);
        }

        public byte[] GetBytes(DbDataReader reader, string column, int size)
        {
            return base.GetBytes((TReader)reader, column, size);
        }
    }
}
