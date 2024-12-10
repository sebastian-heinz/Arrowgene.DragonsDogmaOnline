#nullable enable

using System;
using System.Data;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql
{
    /// <summary>
    /// Operations for SQL type databases.
    /// </summary>
    public abstract class SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        protected const int NoRowsAffected = 0;
        protected const long NoAutoIncrement = 0;

        protected abstract TCon OpenNewConnection();

        protected virtual TCon ReusableConnection { get; set; }

        /// <summary>
        /// Reusing connections can usually only be done in special cases, depending on the database engine:
        /// - One operation at a time.
        /// - Not thread-safe.
        /// If unsure, check DB engine connector documentation or prefer to use <see cref="OpenNewConnection"/>.
        /// </summary>
        /// <returns>An opened, prior-existing connection</returns>
        protected virtual TCon OpenExistingConnection()
        {
            switch (ReusableConnection.State)
            {
                case ConnectionState.Closed:
                    ReusableConnection.Open();
                    break;
                case ConnectionState.Broken:
                    ReusableConnection.Close();
                    ReusableConnection.Open();
                    break;
            }

            return ReusableConnection;
        }

        protected abstract TCom Command(string query, TCon connection);
        protected abstract long AutoIncrement(TCon connection, TCom command);

        public abstract int Upsert(string table, string[] columns, object[] values, string whereColumn,
            object whereValue, out long autoIncrement);

        public bool ExecuteInTransaction(Action<TCon> action)
        {
            using TCon connection = OpenNewConnection();
            using DbTransaction transaction = connection.BeginTransaction();
            try
            {
                action(connection);
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public int ExecuteNonQuery(string query, Action<TCom> nonQueryAction)
        {
            using TCon connection = OpenNewConnection();
            try
            {
                return ExecuteNonQuery(connection, query, nonQueryAction);
            }
            finally
            {
                connection.Close();
            }
        }

        public int ExecuteNonQuery(TCon conn, string query, Action<TCom> nonQueryAction)
        {
            try
            {
                using TCom command = Command(query, conn);
                nonQueryAction(command);
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Exception(ex, query);
                return NoRowsAffected;
            }
        }

        public int ExecuteNonQuery(string query, Action<TCom> nonQueryAction, out long autoIncrement)
        {
            using TCon connection = OpenNewConnection();
            try
            {
                return ExecuteNonQuery(connection, query, nonQueryAction, out autoIncrement);
            }
            finally
            {
                connection.Close();
            }
        }

        public int ExecuteNonQuery(TCon conn, string query, Action<TCom> nonQueryAction, out long autoIncrement)
        {
            try
            {
                using TCom command = Command(query, conn);
                nonQueryAction(command);
                var rowsAffected = command.ExecuteNonQuery();
                autoIncrement = AutoIncrement(conn, command);
                return rowsAffected;
            }
            catch (Exception ex)
            {
                Exception(ex, query);
                autoIncrement = NoAutoIncrement;
                return NoRowsAffected;
            }
        }

        public void ExecuteReader(string query, Action<TCom> nonQueryAction, Action<TReader> readAction)
        {
            using TCon connection = OpenNewConnection();
            try
            {
                ExecuteReader(connection, query, nonQueryAction, readAction);
            }
            finally
            {
                connection.Close();
            }
        }

        public void ExecuteReader(TCon conn, string query, Action<TCom> nonQueryAction, Action<TReader> readAction)
        {
            try
            {
                using TCom command = Command(query, conn);
                nonQueryAction(command);
                using TReader reader = (TReader)command.ExecuteReader();
                readAction(reader);
            }
            catch (Exception ex)
            {
                Exception(ex, query);
            }
        }

        public void ExecuteReader(string query, Action<TReader> readAction)
        {
            using TCon connection = OpenNewConnection();
            try
            {
                ExecuteReader(connection, query, readAction);
            }
            finally
            {
                connection.Close();
            }
        }

        public void ExecuteReader(TCon conn, string query, Action<TReader> readAction)
        {
            try
            {
                using TCom command = Command(query, conn);
                using TReader reader = (TReader)command.ExecuteReader();
                readAction(reader);
            }
            catch (Exception ex)
            {
                Exception(ex, query);
            }
        }

        public void Execute(string query)
        {
            using TCon connection = OpenNewConnection();
            try
            {
                Execute(connection, query);
            }
            finally
            {
                connection.Close();
            }
        }

        public void Execute(TCon? conn, string query)
        {
            try
            {
                using TCom command = Command(query, conn);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Exception(ex, query);
            }
        }

        public string ServerVersion()
        {
            using TCon connection = OpenNewConnection();
            try
            {
                return ServerVersion(connection);
            }
            finally
            {
                connection.Close();
            }
        }

        public string ServerVersion(TCon conn)
        {
            try
            {
                return conn.ServerVersion;
            }
            catch (Exception ex)
            {
                Exception(ex);
                return string.Empty;
            }
        }

        protected virtual void Exception(Exception ex, string? query = null)
        {
            throw ex;
        }

        protected DbParameter Parameter(TCom command, string name, object? value, DbType type)
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

        public void AddParameter(TCom command, string name, object? value, DbType type)
        {
            DbParameter parameter = Parameter(command, name, value, type);
            command.Parameters.Add(parameter);
        }

        public void AddParameter(TCom command, string name, string value)
        {
            AddParameter(command, name, value, DbType.String);
        }

        public void AddParameter(TCom command, string name, Int32 value)
        {
            AddParameter(command, name, value, DbType.Int32);
        }

        public void AddParameter(TCom command, string name, float value)
        {
            AddParameter(command, name, value, DbType.Double);
        }

        public void AddParameter(TCom command, string name, byte value)
        {
            AddParameter(command, name, value, DbType.Byte);
        }

        public void AddParameter(TCom command, string name, UInt16 value)
        {
            AddParameter(command, name, (short)value, DbType.Int16);
        }

        public void AddParameter(TCom command, string name, UInt32 value)
        {
            AddParameter(command, name, (int)value, DbType.Int32);
        }

        public void AddParameterEnumInt32<T>(TCom command, string name, T value) where T : Enum
        {
            AddParameter(command, name, (Int32)(object)value, DbType.Int32);
        }

        public virtual void AddParameter(TCom command, string name, DateTime? value)
        {
            AddParameter(command, name, value, DbType.DateTime);
        }

        public virtual void AddParameter(TCom command, string name, DateTime value)
        {
            AddParameter(command, name, value, DbType.DateTime);
        }

        public void AddParameter(TCom command, string name, byte[] value)
        {
            AddParameter(command, name, value, DbType.Binary);
        }

        public void AddParameter(TCom command, string name, bool value)
        {
            AddParameter(command, name, value, DbType.Boolean);
        }

        public virtual DateTime? GetDateTimeNullable(TReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return reader.GetDateTime(ordinal);
        }

        public string? GetStringNullable(TReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return reader.GetString(ordinal);
        }

        public byte[]? GetBytesNullable(TReader reader, int ordinal, int size)
        {
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            byte[] buffer = new byte[size];
            reader.GetBytes(ordinal, 0, buffer, 0, size);
            return buffer;
        }

        public int GetInt32(TReader reader, string column)
        {
            return reader.GetInt32(reader.GetOrdinal(column));
        }

        public uint GetUInt32(TReader reader, string column)
        {
            return (uint)reader.GetInt32(reader.GetOrdinal(column));
        }

        public byte GetByte(TReader reader, string column)
        {
            return reader.GetByte(reader.GetOrdinal(column));
        }

        public short GetInt16(TReader reader, string column)
        {
            return reader.GetInt16(reader.GetOrdinal(column));
        }

        public ushort GetUInt16(TReader reader, string column)
        {
            return (ushort)reader.GetInt16(reader.GetOrdinal(column));
        }

        public long GetInt64(TReader reader, string column)
        {
            return reader.GetInt64(reader.GetOrdinal(column));
        }

        public ulong GetUInt64(TReader reader, string column)
        {
            return (ulong)reader.GetInt64(reader.GetOrdinal(column));
        }

        public float GetFloat(TReader reader, string column)
        {
            return reader.GetFloat(reader.GetOrdinal(column));
        }

        public string GetString(TReader reader, string column)
        {
            return reader.GetString(reader.GetOrdinal(column));
        }

        public bool GetBoolean(TReader reader, string column)
        {
            return reader.GetBoolean(reader.GetOrdinal(column));
        }

        public T GetEnumInt32<T>(TReader reader, string column) where T : Enum
        {
            return (T)(object)reader.GetInt32(reader.GetOrdinal(column));
        }

        public virtual DateTime GetDateTime(TReader reader, string column)
        {
            return reader.GetDateTime(reader.GetOrdinal(column));
        }

        public byte[] GetBytes(TReader reader, string column, int size)
        {
            byte[] buffer = new byte[size];
            reader.GetBytes(reader.GetOrdinal(column), 0, buffer, 0, size);
            return buffer;
        }

        public DateTime? GetDateTimeNullable(TReader reader, string column)
        {
            int ordinal = reader.GetOrdinal(column);
            return GetDateTimeNullable(reader, ordinal);
        }

        public string? GetStringNullable(TReader reader, string column)
        {
            int ordinal = reader.GetOrdinal(column);
            return GetStringNullable(reader, ordinal);
        }

        public byte[]? GetBytesNullable(TReader reader, string column, int size)
        {
            int ordinal = reader.GetOrdinal(column);
            return GetBytesNullable(reader, ordinal, size);
        }
    }
}
