using System;
using System.Runtime.Serialization;

namespace Arrowgene.Ddon.PacketLibrary
{
    [DataContract]
    public class PlPacket
    {
        [DataMember(Order = 0)] public DateTimeOffset Timestamp { get; set; }
        [DataMember(Order = 1)] public string Direction { get; set; }
        [DataMember(Order = 2)] public byte[] Data { get; set; }
    }
}
