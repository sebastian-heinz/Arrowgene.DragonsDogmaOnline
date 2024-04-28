using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Cli.Command.Packet;

public class PcapPacket
{
    public PacketServerType PacketServerType { get; set; }
    public PacketSource Source { get; set; }
    public string TimeStamp { get; set; }
    public uint Index { get; set; }
    public uint Packet { get; set; }
    public byte[] Data { get; set; }
    public List<IPacket> ResolvedPackets { get; set; }
}
