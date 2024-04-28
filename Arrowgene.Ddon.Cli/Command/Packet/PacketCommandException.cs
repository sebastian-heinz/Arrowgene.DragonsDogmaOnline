using System;

namespace Arrowgene.Ddon.Cli.Command.Packet;

public class PacketCommandException : Exception
{
    public PacketCommandException(string message) : base(message)
    {
    }

    public PacketCommandException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
