using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Arrowgene.Ddon.PacketLibrary
{
    [DataContract]
    public class PlPacketStream
    {
        [DataMember(Order = 0)] public bool Encrypted { get; set; }
        [DataMember(Order = 1)] public string EncryptionKey { get; set; }
        [DataMember(Order = 2)] public DateTimeOffset LogStartTime { get; set; }
        [DataMember(Order = 3)] public string ServerType { get; set; }
        [DataMember(Order = 4)] public string ServerIP { get; set; }
        [DataMember(Order = 5)] public List<PlPacket> Packets { get; set; }
    }
}
