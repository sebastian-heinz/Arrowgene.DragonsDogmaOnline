using System.IO;
using System.Net;
using System.Runtime.Serialization;
using Arrowgene.Ddon.Shared;
using Arrowgene.Networking.Tcp.Server.AsyncEvent;

namespace Arrowgene.Ddon.Server
{
    [DataContract]
    public class ServerSetting
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

        [IgnoreDataMember] public IPAddress ServerIpAddress { get; set; }

        [DataMember(Name = "ServerIpAddress", Order = 1)]
        public string DataServerIpAddress
        {
            get => ServerIpAddress.ToString();
            set => ServerIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [DataMember(Order = 2)] public ushort ServerPort { get; set; }
        [DataMember(Order = 3)] public string Name { get; set; }
        [DataMember(Order = 3)] public string AssetDirectory { get; set; }
        [DataMember(Order = 20)] public int LogLevel { get; set; }
        [DataMember(Order = 21)] public bool LogUnknownPackets { get; set; }
        [DataMember(Order = 22)] public bool LogOutgoingPackets { get; set; }
        [DataMember(Order = 23)] public bool LogOutgoingPacketPayload { get; set; }
        [DataMember(Order = 24)] public bool LogIncomingPackets { get; set; }
        [DataMember(Order = 25)] public bool LogIncomingPacketPayload { get; set; }
        [DataMember(Order = 100)] public AsyncEventSettings ServerSocketSettings { get; set; }

        public ServerSetting()
        {
            ListenIpAddress = IPAddress.Any;
            ServerIpAddress = IPAddress.Loopback;
            ServerPort = 52100;
            Name = "Unknown";
            AssetDirectory = Path.Combine(Util.RelativeExecutingDirectory(), "Files/Assets");
            LogLevel = 0;
            LogUnknownPackets = true;
            LogOutgoingPackets = true;
            LogOutgoingPacketPayload = true;
            LogIncomingPackets = true;
            LogIncomingPacketPayload = true;
            ServerSocketSettings = new AsyncEventSettings();
            ServerSocketSettings.MaxUnitOfOrder = 2;
        }

        public ServerSetting(ServerSetting setting)
        {
            ListenIpAddress = setting.ListenIpAddress;
            ServerIpAddress = setting.ServerIpAddress;
            ServerPort = setting.ServerPort;
            Name = setting.Name;
            AssetDirectory = setting.AssetDirectory;
            LogLevel = setting.LogLevel;
            LogUnknownPackets = setting.LogUnknownPackets;
            LogOutgoingPackets = setting.LogOutgoingPackets;
            LogOutgoingPacketPayload = setting.LogOutgoingPacketPayload;
            LogIncomingPackets = setting.LogIncomingPackets;
            LogIncomingPacketPayload = setting.LogIncomingPacketPayload;
            ServerSocketSettings = new AsyncEventSettings(setting.ServerSocketSettings);
        }
    }
}
