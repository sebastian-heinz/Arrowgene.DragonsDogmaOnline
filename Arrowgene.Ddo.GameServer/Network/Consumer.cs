using System;
using System.Collections.Generic;
using Arrowgene.Ddo.GameServer.Logging;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;
using Arrowgene.Networking.Tcp.Consumer.BlockingQueueConsumption;
using Arrowgene.Networking.Tcp.Server.AsyncEvent;

namespace Arrowgene.Ddo.GameServer.Network
{
    public class Consumer : ThreadedBlockingQueueConsumer
    {
        private static readonly DdoLogger Logger = LogProvider.Logger<DdoLogger>(typeof(Consumer));
        private readonly Dictionary<ushort, IPacketHandler> _packetHandlerLookup;
        private readonly Dictionary<ITcpSocket, Client> _clients;
        private readonly object _lock;
        private readonly GameServerSetting _setting;
        
        public Action<Client> ClientDisconnected;
        public Action<Client> ClientConnected;
        
        public Consumer(GameServerSetting setting, AsyncEventSettings socketSetting, string identity) : base(socketSetting, identity)
        {
            _setting = setting;
            _lock = new object();
            _clients = new Dictionary<ITcpSocket, Client>();
            _packetHandlerLookup = new Dictionary<ushort, IPacketHandler>();
        }

        public void Clear()
        {
            _packetHandlerLookup.Clear();
        }

        public void AddHandler(IPacketHandler packetHandler)
        {
            if (_packetHandlerLookup.ContainsKey(packetHandler.Id))
            {
                Logger.Error($"PacketHandlerId: {packetHandler.Id} already exists");
            }
            else
            {
                _packetHandlerLookup.Add(packetHandler.Id, packetHandler);
            }
        }

        protected override void HandleReceived(ITcpSocket socket, byte[] data)
        {
            Logger.Received(socket, data);
            if (!socket.IsAlive)
            {
                return;
            }

            Client client;
            lock (_lock)
            {
                if (!_clients.ContainsKey(socket))
                {
                    Logger.Error(socket, "Client does not exist in lookup");
                    return;
                }

                client = _clients[socket];
            }

            List<Packet> packets = client.Receive(data);
            foreach (Packet packet in packets)
            {
                HandlePacket(client, packet);
            }
        }

        private void HandlePacket(Client client, Packet packet)
        {
            Logger.LogUnknownIncomingPacket(client, packet);
            if (!_packetHandlerLookup.ContainsKey(packet.Id))
            {
                //Logger.LogUnknownIncomingPacket(client, packet);
                return;
            }

            IPacketHandler packetHandler = _packetHandlerLookup[packet.Id];
            //Logger.LogIncomingPacket(client, packet);
            packet.Data.SetPositionStart();
            try
            {
                packetHandler.Handle(client, packet);
            }
            catch (Exception ex)
            {
                Logger.Exception(client, ex);
            }
        }

        protected override void HandleDisconnected(ITcpSocket socket)
        {
            Client client;
            lock (_lock)
            {
                if (!_clients.ContainsKey(socket))
                {
                    Logger.Error(socket, $"Disconnected client does not exist in lookup");
                    return;
                }

                client = _clients[socket];
                _clients.Remove(socket);
            }
            
            Action<Client> onClientDisconnected = ClientDisconnected;
            if (onClientDisconnected != null)
            {
                try
                {
                    onClientDisconnected.Invoke(client);
                }
                catch (Exception ex)
                {
                    Logger.Exception(client, ex);
                }
            }

            Logger.Info($"Disconnected: {client.Identity}");
        }

        protected override void HandleConnected(ITcpSocket socket)
        {
            Client client = new Client(socket, new PacketFactory(_setting));
            lock (_lock)
            {
                _clients.Add(socket, client);
            }

            Logger.Info($"Connected: {client.Identity}");
            
            Action<Client> onClientConnected = ClientConnected;
            if (onClientConnected != null)
            {
                try
                {
                    onClientConnected.Invoke(client);
                }
                catch (Exception ex)
                {
                    Logger.Exception(client, ex);
                }
            }
        }
    }
}
