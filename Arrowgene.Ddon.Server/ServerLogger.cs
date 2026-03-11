using System;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Networking.SAEAServer;

namespace Arrowgene.Ddon.Server
{
    public class ServerLogger : Logger
    {
        private ServerSetting _serverSetting;

        public override void Initialize(string identity, string name, Action<Log> write)
        {
            base.Initialize(identity, name, write);
        }

        public override void Configure(object loggerTypeConfig, object identityConfig)
        {
            base.Configure(loggerTypeConfig, identityConfig);
            _serverSetting = identityConfig as ServerSetting;
        }

        public void Hex(byte[] data)
        {
            Info($"\n{Util.HexDump(data)}");
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

        public void Error(ClientHandle clientHandle, string message)
        {
            if (clientHandle.TrySnapshot(out ClientSnapshot clientSnapshot))
            {
                Error(clientSnapshot, message);
                return;
            }

            Error($"[Id:{clientHandle.ClientId}][Gen:{clientHandle.Generation}] {message}");
        }

        public void Error(ClientSnapshot clientSnapshot, string message)
        {
            Error($"{clientSnapshot.Identity} {message}");
        }

        public void Exception(ClientHandle clientHandle, Exception exception)
        {
            if (clientHandle.TrySnapshot(out ClientSnapshot clientSnapshot))
            {
                Exception(clientSnapshot, exception);
                return;
            }

            Write(
                LogLevel.Error,
                $"[Id:{clientHandle.ClientId}][Gen:{clientHandle.Generation}] {exception}",
                exception
            );
        }

        public void Exception(ClientSnapshot clientSnapshot, Exception exception)
        {
            Write(LogLevel.Error, $"{clientSnapshot.Identity} {exception}", exception);
        }

        public void LogPacket(Client client, IPacket packet)
        {
            if (_serverSetting == null)
            {
                Error("Can not log packet (_serverSetting == null)");
                return;
            }

            switch (packet.Source)
            {
                case PacketSource.Client:
                {
                    if (!_serverSetting.LogIncomingPackets)
                    {
                        return;
                    }

                    if (_serverSetting.LogIncomingPacketStructure)
                    {
                        if (packet is IStructurePacket structurePacket)
                        {
                            Write(LogLevel.Debug,
                                $"{client.Identity}{Environment.NewLine}{structurePacket.PrintStructure()}", packet);
                            return;
                        }
                    }

                    if (!_serverSetting.LogIncomingPacketPayload)
                    {
                        Write(LogLevel.Debug, $"{client.Identity}{Environment.NewLine}{packet.PrintHeader()}", packet);
                        return;
                    }

                    break;
                }
                case PacketSource.Server:
                {
                    if (!_serverSetting.LogOutgoingPackets)
                    {
                        return;
                    }

                    if (_serverSetting.LogOutgoingPacketStructure)
                    {
                        if (packet is IStructurePacket structurePacket)
                        {
                            Write(LogLevel.Debug,
                                $"{client.Identity}{Environment.NewLine}{structurePacket.PrintStructure()}", packet);
                            return;
                        }
                    }

                    if (!_serverSetting.LogOutgoingPacketPayload)
                    {
                        Write(LogLevel.Debug, $"{client.Identity}{Environment.NewLine}{packet.PrintHeader()}", packet);
                        return;
                    }

                    break;
                }
                default:
                    if (!_serverSetting.LogUnknownPackets)
                    {
                        return;
                    }

                    break;
            }


            Write(LogLevel.Debug, $"{client.Identity}{Environment.NewLine}{packet}", packet);
        }

        public void LogPacketError<TClient>(TClient client, IPacket packet) where TClient : Client
        {
            Write(LogLevel.Error, $"PACKET ERROR: {client.Identity}{Environment.NewLine}{packet}",
                packet);
        }

        public void LogUnhandledPacket<TClient>(TClient client, IPacket packet) where TClient : Client
        {
            if (_serverSetting == null)
            {
                Error("Can not log unhandled packet (_serverSetting == null)");
                return;
            }

            if (!_serverSetting.LogUnknownPackets)
            {
                return;
            }

            Write(LogLevel.Error,
                $"UNHANDLED PACKET:{Environment.NewLine}{client.Identity}{Environment.NewLine}{packet}", packet);
        }
    }
}
