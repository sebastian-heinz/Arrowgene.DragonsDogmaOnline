using System;
using System.Data;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql
{
    /// <summary>
    /// Operations for SQL type databases.
    /// </summary>
    public abstract class SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        public const int NoRowsAffected = 0;
        public const long NoAutoIncrement = 0;

        public SqlDb()
        {
        }

        protected abstract TCon Connection();
        protected abstract TCom Command(string query, TCon connection);
        protected abstract long AutoIncrement(TCon connection, TCom command);

        public abstract int Upsert(string table, string[] columns, object[] values, string whereColumn,
            object whereValue, out long autoIncrement);

        public bool ExecuteInTransaction(Action<TCon> action)
        {
            using (TCon connection = Connection())
            {
                try
                {
                    Execute(connection, "BEGIN TRANSACTION");
                    action(connection);
                    Execute(connection, "COMMIT");
                    return true;
                }
                catch (Exception ex)
                {
                    Execute(connection, "ROLLBACK");
                    Exception(ex);
                    return false;
                }
            }
        }

        public int ExecuteNonQuery(string query, Action<TCom> nonQueryAction)
        {
            return ExecuteNonQuery(null, query, nonQueryAction);
        }

        public int ExecuteNonQuery(TCon conn, string query, Action<TCom> nonQueryAction)
        {
            try
            {
                bool autoCloseConnection = false;
                TCon connection = conn;
                if (connection == null)
                {
                    autoCloseConnection = true;
                    connection = Connection();
                }

                int rowsAffected = 0;
                using (TCom command = Command(query, connection))
                {
                    nonQueryAction(command);
                    rowsAffected = command.ExecuteNonQuery();
                }

                if (autoCloseConnection)
                {
                    connection.Close();
                }

                return rowsAffected;
            }
            catch (Exception ex)
            {
                Exception(ex, query);
                return NoRowsAffected;
            }
        }

        public int ExecuteNonQuery(string query, Action<TCom> nonQueryAction, out long autoIncrement)
        {
            return ExecuteNonQuery(null, query, nonQueryAction, out autoIncrement);
        }

        public int ExecuteNonQuery(TCon conn, string query, Action<TCom> nonQueryAction, out long autoIncrement)
        {
            try
            {
                bool autoCloseConnection = false;
                TCon connection = conn;
                if (connection == null)
                {
                    autoCloseConnection = true;
                    connection = Connection();
                }

                int rowsAffected = 0;
                using (TCom command = Command(query, connection))
                {
                    nonQueryAction(command);
                    rowsAffected = command.ExecuteNonQuery();
                    autoIncrement = AutoIncrement(connection, command);
                }

                if (autoCloseConnection)
                {
                    connection.Close();
                }

                return rowsAffected;
            }
            catch (Exception ex)
            {
                Exception(ex, query);
                autoIncrement = NoAutoIncrement;
                return NoRowsAffected;
            }
        }

        public void ExecuteReader(string query, Action<TCom> nonQueryAction, Action<DbDataReader> readAction)
        {
            ExecuteReader(null, query, nonQueryAction, readAction);
        }

        public void ExecuteReader(TCon conn, string query, Action<TCom> nonQueryAction, Action<DbDataReader> readAction)
        {
            try
            {
                bool autoCloseConnection = false;
                TCon connection = conn;
                if (connection == null)
                {
                    autoCloseConnection = true;
                    connection = Connection();
                }

                using (TCom command = Command(query, connection))
                {
                    nonQueryAction(command);
                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        readAction(reader);
                    }
                }

                if (autoCloseConnection)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Exception(ex, query);
            }
        }

        public void ExecuteReader(string query, Action<DbDataReader> readAction)
        {
            ExecuteReader(null, query, readAction);
        }

        public void ExecuteReader(TCon conn, string query, Action<DbDataReader> readAction)
        {
            try
            {
                bool autoCloseConnection = false;
                TCon connection = conn;
                if (connection == null)
                {
                    autoCloseConnection = true;
                    connection = Connection();
                }

                using (TCom command = Command(query, connection))
                {
                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        readAction(reader);
                    }
                }

                if (autoCloseConnection)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Exception(ex, query);
            }
        }

        public void Execute(string query)
        {
            Execute(null, query);
        }

        public void Execute(TCon conn, string query)
        {
            try
            {
                bool autoCloseConnection = false;
                TCon connection = conn;
                if (connection == null)
                {
                    autoCloseConnection = true;
                    connection = Connection();
                }

                using (TCom command = Command(query, connection))
                {
                    command.ExecuteNonQuery();
                }

                if (autoCloseConnection)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Exception(ex, query);
            }
        }

        public string ServerVersion()
        {
            return ServerVersion(null);
        }

        public string ServerVersion(TCon conn)
        {
            try
            {
                bool autoCloseConnection = false;
                TCon connection = conn;
                if (connection == null)
                {
                    autoCloseConnection = true;
                    connection = Connection();
                }

                string serverVersion = connection.ServerVersion;

                if (autoCloseConnection)
                {
                    connection.Close();
                }

                return serverVersion;
            }
            catch (Exception ex)
            {
                Exception(ex);
                return string.Empty;
            }
        }

        protected virtual void Exception(Exception ex, string query = null)
        {
            throw ex;
        }

        protected DbParameter Parameter(TCom command, string name, object value, DbType type)
        {
            DbParameter parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            parameter.DbType = type;
            return parameter;
        }

        protected DbParameter Parameter(TCom command, string name, string value)
        {
            return Parameter(command, name, value, DbType.String);
        }

        protected void AddParameter(TCom command, string name, object value, DbType type)
        {
            DbParameter parameter = Parameter(command, name, value, type);
            command.Parameters.Add(parameter);
        }

        protected void AddParameter(TCom command, string name, string value)
        {
            AddParameter(command, name, value, DbType.String);
        }

        protected void AddParameter(TCom command, string name, Int32 value)
        {
            AddParameter(command, name, value, DbType.Int32);
        }

        protected void AddParameter(TCom command, string name, float value)
        {
            AddParameter(command, name, value, DbType.Double);
        }

        protected void AddParameter(TCom command, string name, byte value)
        {
            AddParameter(command, name, value, DbType.Byte);
        }

        protected void AddParameter(TCom command, string name, UInt32 value)
        {
            AddParameter(command, name, value, DbType.UInt32);
        }

        protected void AddParameterEnumInt32<T>(TCom command, string name, T value) where T : Enum
        {
            AddParameter(command, name, (Int32)(object)value, DbType.Int32);
        }

        protected void AddParameter(TCom command, string name, DateTime? value)
        {
            AddParameter(command, name, value, DbType.DateTime);
        }

        protected void AddParameter(TCom command, string name, DateTime value)
        {
            AddParameter(command, name, value, DbType.DateTime);
        }

        protected void AddParameter(TCom command, string name, byte[] value)
        {
            AddParameter(command, name, value, DbType.Binary);
        }

        protected void AddParameter(TCom command, string name, bool value)
        {
            AddParameter(command, name, value, DbType.Boolean);
        }

        protected DateTime? GetDateTimeNullable(DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return reader.GetDateTime(ordinal);
        }

        protected string GetStringNullable(DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return reader.GetString(ordinal);
        }

        protected byte[]? GetBytesNullable(DbDataReader reader, int ordinal, int size)
        {
            if(reader.IsDBNull(ordinal))
            {
                return null;
            }

            byte[] buffer = new byte[size];
            reader.GetBytes(ordinal, 0, buffer, 0, size);
            return buffer;
        }

        protected int GetInt32(DbDataReader reader, string column)
        {
            return reader.GetInt32(reader.GetOrdinal(column));
        }

        protected uint GetUInt32(DbDataReader reader, string column)
        {
            return (uint)reader.GetInt32(reader.GetOrdinal(column));
        }

        protected byte GetByte(DbDataReader reader, string column)
        {
            return reader.GetByte(reader.GetOrdinal(column));
        }

        protected short GetInt16(DbDataReader reader, string column)
        {
            return reader.GetInt16(reader.GetOrdinal(column));
        }

        protected ushort GetUInt16(DbDataReader reader, string column)
        {
            return (ushort)reader.GetInt16(reader.GetOrdinal(column));
        }

        protected float GetFloat(DbDataReader reader, string column)
        {
            return reader.GetFloat(reader.GetOrdinal(column));
        }

        protected string GetString(DbDataReader reader, string column)
        {
            return reader.GetString(reader.GetOrdinal(column));
        }

        protected bool GetBoolean(DbDataReader reader, string column)
        {
            return reader.GetBoolean(reader.GetOrdinal(column));
        }

        protected T GetEnumInt32<T>(DbDataReader reader, string column) where T : Enum
        {
            return (T)(object)reader.GetInt32(reader.GetOrdinal(column));
        }

        protected DateTime GetDateTime(DbDataReader reader, string column)
        {
            return reader.GetDateTime(reader.GetOrdinal(column));
        }

        protected byte[] GetBytes(DbDataReader reader, string column, int size)
        {
            byte[] buffer = new byte[size];
            reader.GetBytes(reader.GetOrdinal(column), 0, buffer, 0, size);
            return buffer;
        }

        protected DateTime? GetDateTimeNullable(DbDataReader reader, string column)
        {
            int ordinal = reader.GetOrdinal(column);
            return GetDateTimeNullable(reader, ordinal);
        }

        protected string GetStringNullable(DbDataReader reader, string column)
        {
            int ordinal = reader.GetOrdinal(column);
            return GetStringNullable(reader, ordinal);
        }

        protected byte[] GetBytesNullable(DbDataReader reader, string column, int size)
        {
            int ordinal = reader.GetOrdinal(column);
            return GetBytesNullable(reader, ordinal, size);
        }
    }
}
