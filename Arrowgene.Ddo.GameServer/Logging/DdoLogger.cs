using System;
using Arrowgene.Buffers;
using Arrowgene.Ddo.GameServer.Network;
using Arrowgene.Ddo.Shared;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ddo.GameServer.Logging
{
    public class DdoLogger : Logger
    {
        public override void Initialize(string identity, string name, Action<Log> write, object configuration)
        {
            base.Initialize(identity, name, write, configuration);
        }
        public void Hex(byte[] data)
        {
            Info($"{Util.HexDump(data)}");
        }
        
        public void Info(Client client, string message)
        {
            Info($"{client.Identity} {message}");
        }

        public void Debug(Client client, string message)
        {
            Debug($"{client.Identity} {message}");
        }

        public void Error(Client client, string message)
        {
            Error($"{client.Identity} {message}");
        }

        public void Exception(Client client, Exception exception)
        {
            if (exception == null)
            {
                Write(LogLevel.Error, $"{client.Identity} Exception was null.", null);
            }
            else
            {
                Write(LogLevel.Error, $"{client.Identity} {exception}", exception);
            }
        }

        public void Info(ITcpSocket socket, string message)
        {
            Info($"[{socket.Identity}] {message}");
        }

        public void Debug(ITcpSocket socket, string message)
        {
            Debug($"[{socket.Identity}] {message}");
        }

        public void Error(ITcpSocket socket, string message)
        {
            Error($"[{socket.Identity}] {message}");
        }

        public void Exception(ITcpSocket socket, Exception exception)
        {
            if (exception == null)
            {
                Write(LogLevel.Error, $"{socket.Identity} Exception was null.", null);
            }
            else
            {
                Write(LogLevel.Error, $"{socket.Identity} {exception}", exception);
            }
        }

        public void LogUnknownIncomingPacket(Client client, Packet packet)
        {
            // todo packet log wrapper class
            Write(LogLevel.Debug,
                $"{client.Identity} - {packet.Id} {Environment.NewLine}{Util.HexDump(packet.Data.GetAllBytes())}", packet);
        }

        public void Received(ITcpSocket socket, byte[] data)
        {
            Write(LogLevel.Debug,$"Received: {data.Length}bytes from {socket.Identity}{Environment.NewLine}{Util.HexDump(data)}", data);
        }
        
        public void Send(ITcpSocket socket, byte[] data)
        {
            Write(LogLevel.Debug,$"Send: {data.Length}bytes to {socket.Identity}{Environment.NewLine}{Util.HexDump(data)}", data);
        }
    }
}
