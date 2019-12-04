using System;
using Arrowgene.Services.Logging;
using Arrowgene.Services.Networking.Tcp;
using Ddo.Server.Model;
using Ddo.Server.Packet;
using Ddo.Server.Setting;

namespace Ddo.Server.Logging
{
    public class DdoLogger : Logger
    {
        private DdoSetting _setting;

        public DdoLogger() : this(null)
        {
        }

        public DdoLogger(string identity, string zone = null) : base(identity, zone)
        {
        }

        public override void Initialize(string identity, string zone, object configuration)
        {
            base.Initialize(identity, zone, configuration);
            _setting = configuration as DdoSetting;
            if (_setting == null)
            {
                Error("Couldn't apply DdoLogger configuration");
            }
        }

        public void Info(DdoClient client, string message, params object[] args)
        {
            Write(LogLevel.Info, null, $"{client.Identity} {message}", args);
        }

        public void Info(DdoConnection connection, string message, params object[] args)
        {
            DdoClient client = connection.Client;
            if (client != null)
            {
                Info(client, message, args);
                return;
            }

            Write(LogLevel.Info, null, $"{connection.Identity} {message}", args);
        }

        public void Debug(DdoClient client, string message, params object[] args)
        {
            Write(LogLevel.Debug, null, $"{client.Identity} {message}", args);
        }

        public void Error(DdoClient client, string message, params object[] args)
        {
            Write(LogLevel.Error, null, $"{client.Identity} {message}", args);
        }

        public void Error(DdoConnection connection, string message, params object[] args)
        {
            DdoClient client = connection.Client;
            if (client != null)
            {
                Error(client, message, args);
                return;
            }

            Write(LogLevel.Error, null, $"{connection.Identity} {message}", args);
        }

        public void Exception(DdoClient client, Exception exception)
        {
            Write(LogLevel.Error, null, $"{client.Identity} {exception}");
        }

        public void Exception(DdoConnection connection, Exception exception)
        {
            DdoClient client = connection.Client;
            if (client != null)
            {
                Exception(client, exception);
                return;
            }

            Write(LogLevel.Error, null, $"{connection.Identity} {exception}");
        }

        public void Info(ITcpSocket socket, string message, params object[] args)
        {
            Write(LogLevel.Info, null, $"[{socket.Identity}] {message}", args);
        }

        public void Debug(ITcpSocket socket, string message, params object[] args)
        {
            Write(LogLevel.Debug, null, $"[{socket.Identity}] {message}", args);
        }

        public void Error(ITcpSocket socket, string message, params object[] args)
        {
            Write(LogLevel.Error, null, $"[{socket.Identity}] {message}", args);
        }

        public void Exception(ITcpSocket socket, Exception exception)
        {
            Write(LogLevel.Error, null, $"[{socket.Identity}] {exception}");
        }

        public void LogIncomingPacket(DdoClient client, DdoPacket packet)
        {
            if (_setting.LogIncomingPackets)
            {
                DdoLogPacket logPacket = new DdoLogPacket(client.Identity, packet, DdoLogType.PacketIn);
                WritePacket(logPacket);
            }
        }

        public void LogIncomingPacket(DdoConnection connection, DdoPacket packet)
        {
            DdoClient client = connection.Client;
            if (client != null)
            {
                LogIncomingPacket(client, packet);
                return;
            }

            if (!_setting.LogIncomingPackets)
            {
                return;
            }

            DdoLogPacket logPacket = new DdoLogPacket(connection.Identity, packet, DdoLogType.PacketIn);
            WritePacket(logPacket);
        }

        public void LogUnknownIncomingPacket(DdoClient client, DdoPacket packet)
        {
            if (_setting.LogUnknownIncomingPackets)
            {
                DdoLogPacket logPacket = new DdoLogPacket(client.Identity, packet, DdoLogType.PacketUnhandled);
                WritePacket(logPacket);
            }
        }

        public void LogUnknownIncomingPacket(DdoConnection connection, DdoPacket packet)
        {
            DdoClient client = connection.Client;
            if (client != null)
            {
                LogUnknownIncomingPacket(client, packet);
                return;
            }

            if (!_setting.LogIncomingPackets)
            {
                return;
            }

            DdoLogPacket logPacket =
                new DdoLogPacket(connection.Identity, packet, DdoLogType.PacketUnhandled);
            WritePacket(logPacket);
        }

        public void LogOutgoingPacket(DdoClient client, DdoPacket packet)
        {
            if (_setting.LogOutgoingPackets)
            {
                DdoLogPacket logPacket = new DdoLogPacket(client.Identity, packet, DdoLogType.PacketOut);
                WritePacket(logPacket);
            }
        }

        public void LogOutgoingPacket(DdoConnection connection, DdoPacket packet)
        {
            DdoClient client = connection.Client;
            if (client != null)
            {
                LogOutgoingPacket(client, packet);
                return;
            }

            if (!_setting.LogIncomingPackets)
            {
                return;
            }

            DdoLogPacket logPacket = new DdoLogPacket(connection.Identity, packet, DdoLogType.PacketOut);
            WritePacket(logPacket);
        }

        private void WritePacket(DdoLogPacket packet)
        {
            Write(LogLevel.Info, packet, packet.ToLogText());
        }
    }
}
