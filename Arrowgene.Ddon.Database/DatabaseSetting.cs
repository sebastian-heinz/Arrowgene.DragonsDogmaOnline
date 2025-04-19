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
            WipeOnStartup = false;
            // SQLite-only
            EnableTracing = false;
            // PSQL-only
            BufferSize = 32768;
            ResetOnClose = false;

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

            string envDbWipeOnStartup = Environment.GetEnvironmentVariable("DB_WIPE_ON_STARTUP");
            if (!string.IsNullOrEmpty(envDbWipeOnStartup))
            {
                WipeOnStartup = Convert.ToBoolean(envDbWipeOnStartup);
            }

            // SQLite-only
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
            string envDbResetOnClose = Environment.GetEnvironmentVariable("DB_RESET_ON_CLOSE");
            if (!string.IsNullOrEmpty(envDbResetOnClose))
            {
                ResetOnClose = Convert.ToBoolean(envDbResetOnClose);
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
            
            // SQLite-only
            EnableTracing = databaseSettings.EnableTracing;
            
            // PSQL-only
            BufferSize = databaseSettings.BufferSize;
            ResetOnClose = databaseSettings.ResetOnClose;
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
        /// SQLite-only setting to enable increased logging.
        /// </summary>
        [DataMember(Order = 8)] public bool EnableTracing { get; set; }
        
        /// <summary>
        /// PSQL-only setting to increase read/write/socket buffers.
        /// https://www.npgsql.org/doc/performance.html#reading-large-values
        /// </summary>
        [DataMember(Order = 9)] public uint BufferSize { get; set; }
        
        /// <summary>
        /// PSQL-only setting to control "DISCARD ALL" usage.
        /// https://www.npgsql.org/doc/performance.html#pooled-connection-reset
        /// </summary>
        [DataMember(Order = 10)] public bool ResetOnClose { get; set; }
    }
}
