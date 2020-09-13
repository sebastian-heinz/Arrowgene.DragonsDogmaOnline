using System.Net;
using System.Runtime.Serialization;
using Arrowgene.Networking.Tcp.Server.AsyncEvent;

namespace Arrowgene.Ddo.GameServer
{
    [DataContract]
    public class GameServerSetting
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
        [DataMember(Order = 10)] public bool NeedRegistration { get; set; }
        [DataMember(Order = 20)] public int LogLevel { get; set; }
        [DataMember(Order = 21)] public bool LogUnknownIncomingPackets { get; set; }
        [DataMember(Order = 22)] public bool LogOutgoingPackets { get; set; }
        [DataMember(Order = 23)] public bool LogIncomingPackets { get; set; }
        [DataMember(Order = 100)] public AsyncEventSettings AuthServerSocketSettings { get; set; }

        public GameServerSetting()
        {
            ListenIpAddress = IPAddress.Any;
            AuthServerIpAddress = IPAddress.Loopback;
            AuthServerPort = 52100;
            NeedRegistration = false;
            LogLevel = 0;
            LogUnknownIncomingPackets = true;
            LogOutgoingPackets = true;
            LogIncomingPackets = true;
            AuthServerSocketSettings = new AsyncEventSettings();
            AuthServerSocketSettings.MaxUnitOfOrder = 2;
        }

        public GameServerSetting(GameServerSetting setting)
        {
            ListenIpAddress = setting.ListenIpAddress;
            AuthServerIpAddress = setting.AuthServerIpAddress;
            AuthServerPort = setting.AuthServerPort;
            NeedRegistration = setting.NeedRegistration;
            LogLevel = setting.LogLevel;
            LogUnknownIncomingPackets = setting.LogUnknownIncomingPackets;
            LogOutgoingPackets = setting.LogOutgoingPackets;
            LogIncomingPackets = setting.LogIncomingPackets;
            AuthServerSocketSettings = new AsyncEventSettings(setting.AuthServerSocketSettings);
        }
    }
}
