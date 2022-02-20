using System;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ddon.Server
{
    public class ServerLogger : Logger
    {
        private ServerSetting _setting;

        public override void Initialize(string identity, string name, Action<Log> write, object configuration)
        {
            base.Initialize(identity, name, write, configuration);
            _setting = configuration as ServerSetting;
            if (_setting == null)
            {
                Error("Couldn't apply DdonLogger configuration");
            }
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
        
        public void LogPacket(Client client, IPacket packet)
        {
            switch (packet.Source)
            {
                case PacketSource.Client:
                {
                    if (!_setting.LogIncomingPackets)
                    {
                        return;
                    }

                    if (_setting.LogIncomingPacketStructure)
                    {
                        if (packet is IStructurePacket structurePacket)
                        {
                            Write(LogLevel.Debug, $"{client.Identity}{Environment.NewLine}{structurePacket.PrintStructure()}", packet);
                        }
                    }

                    if (!_setting.LogIncomingPacketPayload)
                    {
                        Write(LogLevel.Debug, $"{client.Identity}{Environment.NewLine}{packet.PrintHeader()}", packet);
                        return;
                    }

                    break;
                }
                case PacketSource.Server:
                {
                    if (!_setting.LogOutgoingPackets)
                    {
                        return;
                    }

                    if (_setting.LogOutgoingPacketStructure)
                    {
                        if (packet is IStructurePacket structurePacket)
                        {
                            Write(LogLevel.Debug, $"{client.Identity}{Environment.NewLine}{structurePacket.PrintStructure()}", packet);
                        }
                    }
                    
                    if (!_setting.LogOutgoingPacketPayload)
                    {
                        Write(LogLevel.Debug, $"{client.Identity}{Environment.NewLine}{packet.PrintHeader()}", packet);
                        return;
                    }

                    break;
                }
                default:
                    if (!_setting.LogUnknownPackets)
                    {
                        return;
                    }

                    break;
            }


            Write(LogLevel.Debug, $"{client.Identity}{Environment.NewLine}{packet}", packet);
        }
    }
}
