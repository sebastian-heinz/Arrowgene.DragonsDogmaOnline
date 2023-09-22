using System;
using System.IO;
using System.Runtime.Serialization;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared;

namespace Arrowgene.Ddon.Database
{
    [DataContract]
    public class DatabaseSetting
    {
        public DatabaseSetting()
        {
            Type = DatabaseType.SQLite;
            DatabaseFolder = Path.Combine(Util.ExecutingDirectory(), "Files/Database");
            Host = "localhost";
            Port = 3306;
            Database = "Ddon";
            User = string.Empty;
            Password = string.Empty;
            WipeOnStartup = true;

            if (Enum.TryParse(Environment.GetEnvironmentVariable("DB_TYPE"), true, out DatabaseType dbType))
            {
                Type = dbType;
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
        }

        [DataMember(Order = 0)] public DatabaseType Type { get; set; }

        [DataMember(Order = 1)] public string DatabaseFolder { get; set; }

        [DataMember(Order = 2)] public string Host { get; set; }

        [DataMember(Order = 3)] public short Port { get; set; }

        [DataMember(Order = 4)] public string User { get; set; }

        [DataMember(Order = 5)] public string Password { get; set; }

        [DataMember(Order = 6)] public string Database { get; set; }

        [DataMember(Order = 6)] public bool WipeOnStartup { get; set; }
    }
}
