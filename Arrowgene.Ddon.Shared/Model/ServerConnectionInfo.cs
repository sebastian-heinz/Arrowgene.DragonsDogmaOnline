using System.Net;
using System.Runtime.Serialization;

namespace Arrowgene.Ddon.Shared.Model;

[DataContract]
public class ServerConnectionInfo
{
    [DataMember(Order = 0)] public string Name { get; set; }
    [IgnoreDataMember] public IPAddress IpAddress { get; set; }

    [DataMember(Name = "IpAddress", Order = 1)]
    public string DataIpAddress
    {
        get => IpAddress.ToString();
        set => IpAddress = string.IsNullOrEmpty(value) ? null : IPAddress.Parse(value);
    }

    [DataMember(Order = 2)] public ushort Port { get; set; }

    public ServerConnectionInfo()
    {
        IpAddress = IPAddress.Loopback;
        Port = 52100;
        Name = "Unknown";
    }

    public ServerConnectionInfo(ServerConnectionInfo connectionInfo)
    {
        IpAddress = connectionInfo.IpAddress;
        Port = connectionInfo.Port;
        Name = connectionInfo.Name;
    }
}
