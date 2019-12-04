using System.IO;
using System.Net;
using System.Runtime.Serialization;
using Arrowgene.Services.Networking.Tcp.Server.AsyncEvent;
using Ddo.Server.Common;

namespace Ddo.Server.Setting
{
    [DataContract]
    public class DdoSetting
    {
        /// <summary>
        /// Warning:
        /// Changing while having existing accounts requires to rehash all passwords.
        /// The number is log2, so adding +1 doubles the time it takes.
        /// https://wildlyinaccurate.com/bcrypt-choosing-a-work-factor/
        /// </summary>
        public const int BCryptWorkFactor = 10;

        [IgnoreDataMember] public IPAddress ListenIpAddress { get; set; }

        [DataMember(Name = "ListenIpAddress", Order = 0)]
        public string DataListenIpAddress
        {
            get => ListenIpAddress.ToString();
            set => ListenIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [IgnoreDataMember] public IPAddress AuthServerIpAddress { get; set; }

        [DataMember(Name = "AuthIpAddress", Order = 1)]
        public string DataAuthIpAddress
        {
            get => AuthServerIpAddress.ToString();
            set => AuthServerIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [DataMember(Order = 2)] public ushort AuthServerPort { get; set; }

        [IgnoreDataMember] public IPAddress LobbyServerIpAddress { get; set; }

        [DataMember(Name = "LobbyIpAddress", Order = 3)]
        public string DataLobbyIpAddress
        {
            get => LobbyServerIpAddress.ToString();
            set => LobbyServerIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [DataMember(Order = 4)] public ushort LobbyServerPort { get; set; }

        [DataMember(Order = 10)] public bool NeedRegistration { get; set; }

        [DataMember(Order = 20)] public int LogLevel { get; set; }

        [DataMember(Order = 21)] public bool LogUnknownIncomingPackets { get; set; }

        [DataMember(Order = 22)] public bool LogOutgoingPackets { get; set; }

        [DataMember(Order = 23)] public bool LogIncomingPackets { get; set; }

        [DataMember(Order = 40)] public string FilesFolder { get; set; }

        [DataMember(Order = 60)] public WebSetting WebSetting { get; set; }
        [DataMember(Order = 70)] public DatabaseSetting DatabaseSetting { get; set; }

        [DataMember(Order = 100)] public AsyncEventSettings ServerSocketSettings { get; set; }

        public DdoSetting()
        {
            ListenIpAddress = IPAddress.Any;
            AuthServerIpAddress = IPAddress.Loopback;
            AuthServerPort = 53312;
            LobbyServerIpAddress = IPAddress.Loopback;
            LobbyServerPort = 53310;
            NeedRegistration = false;
            LogLevel = 0;
            LogUnknownIncomingPackets = true;
            LogOutgoingPackets = true;
            LogIncomingPackets = true;
            WebSetting = new WebSetting();
            DatabaseSetting = new DatabaseSetting();
            ServerSocketSettings = new AsyncEventSettings();
            ServerSocketSettings.MaxUnitOfOrder = 2;
            FilesFolder = Path.Combine(Util.ExecutingDirectory(), "Files");
        }

        public DdoSetting(DdoSetting setting)
        {
            ListenIpAddress = setting.ListenIpAddress;
            AuthServerIpAddress = setting.AuthServerIpAddress;
            AuthServerPort = setting.AuthServerPort;
            LobbyServerIpAddress = setting.LobbyServerIpAddress;
            LobbyServerPort = setting.LobbyServerPort;
            NeedRegistration = setting.NeedRegistration;
            LogLevel = setting.LogLevel;
            LogUnknownIncomingPackets = setting.LogUnknownIncomingPackets;
            LogOutgoingPackets = setting.LogOutgoingPackets;
            LogIncomingPackets = setting.LogIncomingPackets;
            WebSetting = setting.WebSetting;
            FilesFolder = setting.FilesFolder;
            DatabaseSetting = new DatabaseSetting(setting.DatabaseSetting);
            ServerSocketSettings = new AsyncEventSettings(setting.ServerSocketSettings);
        }
    }
}
