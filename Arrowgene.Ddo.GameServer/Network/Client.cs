using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddo.GameServer.Logging;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ddo.GameServer.Network
{
    public class Client
    {
        private static readonly DdoLogger Logger = LogProvider.Logger<DdoLogger>(typeof(Client));

        private ITcpSocket _socket;
        private PacketFactory _packetFactory;

        public Client(ITcpSocket socket, PacketFactory packetFactory)
        {
            _socket = socket;
            _packetFactory = packetFactory;
        }

        public string Identity { get; private set; }

        public List<Packet> Receive(byte[] data)
        {
            List<Packet> packets;
            try
            {
                packets = _packetFactory.Read(data);
            }
            catch (Exception ex)
            {
                Logger.Exception(this, ex);
                packets = new List<Packet>();
            }

            return packets;
        }

        public void Send(ushort id, IBuffer buffer)
        {
            Packet packet = new Packet(id, buffer);
            Send(packet);
        }

        public void Send(Packet packet)
        {
            byte[] data;
            try
            {
                data = _packetFactory.Write(packet);
            }
            catch (Exception ex)
            {
                Logger.Exception(this, ex);
                return;
            }

            Send(data);
        }

        public void Send(byte[] data)
        {
            // Logger.LogOutgoingPacket(this, packet, ServerType);
            _socket.Send(data);
        }
    }
}
