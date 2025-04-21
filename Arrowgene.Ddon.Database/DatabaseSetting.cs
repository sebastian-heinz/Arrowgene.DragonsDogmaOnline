using System;
using System.IO;
using System.Runtime.Serialization;
using Arrowgene.Ddon.Shared;

namespace Arrowgene.Ddon.Database
{
    [DataContract]
    public class DatabaseSetting
    {
        public DatabaseSetting()
        {
            Type = "sqlite";
            DatabaseFolder = Path.Combine(Util.ExecutingDirectory(), "Files/Database");
            Host = "localhost";
            Port = 3306;
            Database = "Ddon";
            User = string.Empty;
            Password = string.Empty;
            EnablePooling = true;
            EnableTracing = true;
            BufferSize = 32768;
            // SQLite-only
            WipeOnStartup = false;
            // PSQL-only
            NoResetOnClose = true;
            // PSQL-only
            MaxAutoPrepare = 200;

            string envDbType = Environment.GetEnvironmentVariable("DB_TYPE");
            if (!string.IsNullOrEmpty(envDbType))
            {
                Type = envDbType;
            }

            string envDbFolder = Environment.GetEnvironmentVariable("DB_FOLDER");
            if (!string.IsNullOrEmpty(envDbFolder))
            {
                DatabaseFolder = envDbFolder;
            }

            string envDbHost = Environment.GetEnvironmentVariable("DB_HOST");
            if (!string.IsNullOrEmpty(envDbHost))
            {
                Host = envDbHost;
            }

            string envDbPort = Environment.GetEnvironmentVariable("DB_PORT");
            if (!string.IsNullOrEmpty(envDbPort))
            {
                Port = Convert.ToInt16(envDbPort);
            }

            string envDbDatabase = Environment.GetEnvironmentVariable("DB_DATABASE");
            if (!string.IsNullOrEmpty(envDbDatabase))
            {
                Database = envDbDatabase;
            }

            string envDbUser = Environment.GetEnvironmentVariable("DB_USER");
            if (!string.IsNullOrEmpty(envDbUser))
            {
                User = envDbUser;
            }

            string envDbPass = Environment.GetEnvironmentVariable("DB_PASS");
            if (!string.IsNullOrEmpty(envDbPass))
            {
                Password = envDbPass;
            }

            string envDbEnablePooling = Environment.GetEnvironmentVariable("DB_ENABLE_POOLING");
            if (!string.IsNullOrEmpty(envDbEnablePooling))
            {
                EnablePooling = Convert.ToBoolean(envDbEnablePooling);
            }

            // SQLite-only
            string envDbWipeOnStartup = Environment.GetEnvironmentVariable("DB_WIPE_ON_STARTUP");
            if (!string.IsNullOrEmpty(envDbWipeOnStartup))
            {
                WipeOnStartup = Convert.ToBoolean(envDbWipeOnStartup);
            }

            string envDbEnableTracing = Environment.GetEnvironmentVariable("DB_ENABLE_TRACING");
            if (!string.IsNullOrEmpty(envDbEnableTracing))
            {
                EnableTracing = Convert.ToBoolean(envDbEnableTracing);
            }

            // PSQL-only
            string envDbBufferSize = Environment.GetEnvironmentVariable("DB_BUFFER_SIZE");
            if (!string.IsNullOrEmpty(envDbBufferSize))
            {
                BufferSize = Convert.ToUInt32(envDbBufferSize);
            }

            // PSQL-only
            string envDbResetOnClose = Environment.GetEnvironmentVariable("DB_NO_RESET_ON_CLOSE");
            if (!string.IsNullOrEmpty(envDbResetOnClose))
            {
                NoResetOnClose = Convert.ToBoolean(envDbResetOnClose);
            }

            // PSQL-only
            string envDbMaxAutoPrepare = Environment.GetEnvironmentVariable("DB_MAX_AUTO_PREPARE");
            if (!string.IsNullOrEmpty(envDbMaxAutoPrepare))
            {
                MaxAutoPrepare = Convert.ToUInt32(envDbMaxAutoPrepare);
            }
        }

        public DatabaseSetting(DatabaseSetting databaseSettings)
        {
            Type = databaseSettings.Type;
            DatabaseFolder = databaseSettings.DatabaseFolder;
            Host = databaseSettings.Host;
            Port = databaseSettings.Port;
            User = databaseSettings.User;
            Password = databaseSettings.Password;
            Database = databaseSettings.Database;
            WipeOnStartup = databaseSettings.WipeOnStartup;
            BufferSize = databaseSettings.BufferSize;

            // SQLite-only
            EnableTracing = databaseSettings.EnableTracing;

            // PSQL-only
            NoResetOnClose = databaseSettings.NoResetOnClose;
            MaxAutoPrepare = databaseSettings.MaxAutoPrepare;
        }

        [DataMember(Order = 0)] public string Type { get; set; }

        [DataMember(Order = 1)] public string DatabaseFolder { get; set; }

        [DataMember(Order = 2)] public string Host { get; set; }

        [DataMember(Order = 3)] public short Port { get; set; }

        [DataMember(Order = 4)] public string User { get; set; }

        [DataMember(Order = 5)] public string Password { get; set; }

        [DataMember(Order = 6)] public string Database { get; set; }

        [DataMember(Order = 7)] public bool WipeOnStartup { get; set; }

        /// <summary>
        /// Behavior differs per DB:
        /// - SQLite: Enable increased logging via trace events.
        /// - PSQL: Enable IncludeErrorDetail.
        /// </summary>
        [DataMember(Order = 8)]
        public bool EnableTracing { get; set; }

        /// <summary>
        /// Behavior differs per DB:
        /// - SQLite: Adjust cache size in kibibytes. See: https://sqlite.org/pragma.html#pragma_cache_size
        /// - PSQL: Adjust read, write, socket receive and socket send buffers. See: https://www.npgsql.org/doc/performance.html#reading-large-values
        /// </summary>
        [DataMember(Order = 9)]
        public uint BufferSize { get; set; }

        /// <summary>
        /// PSQL-only setting to control "DISCARD ALL" usage.
        /// https://www.npgsql.org/doc/performance.html#pooled-connection-reset
        /// https://www.npgsql.org/doc/compatibility.html#pgbouncer
        /// Set this to true when using PgBouncer.
        /// </summary>
        [DataMember(Order = 10)]
        public bool NoResetOnClose { get; set; }

        /// <summary>
        /// Whether to enable built-in pooling at driver-level.
        /// For use cases where an external connection pool exists e.g. PgBouncer this should be turned off.
        /// </summary>
        [DataMember(Order = 11)]
        public bool EnablePooling { get; set; }

        /// <summary>
        /// PSQL-only setting to control how many statements to prepare and keep at most.
        /// https://www.npgsql.org/doc/prepare.html#automatic-preparation
        /// </summary>
        [DataMember(Order = 12)]
        public uint MaxAutoPrepare { get; set; }
    }
}
