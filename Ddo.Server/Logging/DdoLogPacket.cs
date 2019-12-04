using System;
using Ddo.Server.Packet;

namespace Ddo.Server.Logging
{
    public class DdoLogPacket : DdoPacket
    {
        public DdoLogPacket(string clientIdentity, DdoPacket packet, DdoLogType logType)
            : base(packet.Header, packet.Data.Clone())
        {
            LogType = logType;
            TimeStamp = DateTime.Now;
            ClientIdentity = clientIdentity;
        }

        public string ClientIdentity { get; }
        public DdoLogType LogType { get; }
        public DateTime TimeStamp { get; }
        public string Hex => Data.ToHexString(' ');
        public string Ascii => Data.ToAsciiString(true);

        public string ToLogText()
        {
            String log = $"{ClientIdentity} Packet Log";
            log += Environment.NewLine;
            log += "----------";
            log += Environment.NewLine;
            log += $"[{TimeStamp:HH:mm:ss}][Typ:{LogType}]";
            log += Environment.NewLine;
            log += $"[Id:0x{Id:X2}|{Id}][BodyLen:{Data.Size}][{PacketIdName}]";
            log += Environment.NewLine;
            log += Header.ToLogText();
            log += Environment.NewLine;
            log += "ASCII:";
            log += Environment.NewLine;
            log += Ascii;
            log += Environment.NewLine;
            log += "HEX:";
            log += Environment.NewLine;
            log += Hex;
            log += Environment.NewLine;
            log += "----------";
            return log;
        }
    }
}
