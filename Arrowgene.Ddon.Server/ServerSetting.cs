using System.Net;
using System.Runtime.Serialization;
using Arrowgene.Networking.Tcp.Server.AsyncEvent;

namespace Arrowgene.Ddon.Server
{
    [DataContract]
    public class ServerSetting
    {
        public const int InvalidId = -1;

        [DataMember(Order = 0)] public int Id { get; set; }
        [DataMember(Order = 1)] public string Name { get; set; }
        [IgnoreDataMember] public IPAddress ListenIpAddress { get; set; }

        [DataMember(Name = "ListenIpAddress", Order = 5)]
        public string DataListenIpAddress
        {
            get => ListenIpAddress.ToString();
            set => ListenIpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
        }

        [DataMember(Order = 6)] public ushort ServerPort { get; set; }
        [DataMember(Order = 20)] public int LogLevel { get; set; }
        [DataMember(Order = 21)] public bool LogUnknownPackets { get; set; }
        [DataMember(Order = 22)] public bool LogOutgoingPackets { get; set; }
        [DataMember(Order = 23)] public bool LogOutgoingPacketPayload { get; set; }
        [DataMember(Order = 24)] public bool LogIncomingPackets { get; set; }
        [DataMember(Order = 25)] public bool LogIncomingPacketPayload { get; set; }
        [DataMember(Order = 26)] public bool LogIncomingPacketStructure { get; set; }
        [DataMember(Order = 27)] public bool LogOutgoingPacketStructure { get; set; }
        [DataMember(Order = 100)] public AsyncEventSettings ServerSocketSettings { get; set; }

        public ServerSetting()
        {
            Id = InvalidId;
            Name = "";
            ListenIpAddress = IPAddress.Any;
            ServerPort = 52100;
            LogLevel = 0;
            LogUnknownPackets = true;
            LogOutgoingPackets = true;
            LogOutgoingPacketStructure = false;
            LogOutgoingPacketPayload = false;
            LogIncomingPackets = true;
            LogIncomingPacketStructure = false;
            LogIncomingPacketPayload = false;
            ServerSocketSettings = new AsyncEventSettings();
            ServerSocketSettings.MaxUnitOfOrder = 1;
        }

        public ServerSetting(ServerSetting setting)
        {
            Id = setting.Id;
            Name = setting.Name;
            ListenIpAddress = setting.ListenIpAddress;
            ServerPort = setting.ServerPort;
            LogLevel = setting.LogLevel;
            LogUnknownPackets = setting.LogUnknownPackets;
            LogOutgoingPackets = setting.LogOutgoingPackets;
            LogOutgoingPacketStructure = setting.LogOutgoingPacketStructure;
            LogOutgoingPacketPayload = setting.LogOutgoingPacketPayload;
            LogIncomingPackets = setting.LogIncomingPackets;
            LogIncomingPacketStructure = setting.LogIncomingPacketStructure;
            LogIncomingPacketPayload = setting.LogIncomingPacketPayload;
            ServerSocketSettings = new AsyncEventSettings(setting.ServerSocketSettings);
        }

        // Note: method is called after the object is completely deserialized - constructors are skipped.
        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            ServerSocketSettings ??= new AsyncEventSettings();
        }
    }
}
