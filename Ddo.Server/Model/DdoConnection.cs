using System;
using System.Collections.Generic;
using Arrowgene.Services.Logging;
using Arrowgene.Services.Networking.Tcp;
using Ddo.Server.Logging;
using Ddo.Server.Packet;

namespace Ddo.Server.Model
{
    public class DdoConnection : ISender
    {
        private readonly DdoLogger _logger;

        public DdoConnection(ITcpSocket clientSocket, PacketFactory packetFactory)
        {
            _logger = LogProvider.Logger<DdoLogger>(this);
            Socket = clientSocket;
            PacketFactory = packetFactory;
            Client = null;
        }

        public string Identity => Socket.Identity;
        public ITcpSocket Socket { get; }
        public PacketFactory PacketFactory { get; }
        public DdoClient Client { get; set; }

        public List<DdoPacket> Receive(byte[] data)
        {
            List<DdoPacket> packets;
            try
            {
                packets = PacketFactory.Read(data);
            }
            catch (Exception ex)
            {
                _logger.Exception(this, ex);
                packets = new List<DdoPacket>();
            }

            return packets;
        }

        public void Send(DdoPacket packet)
        {
            byte[] data;
            try
            {
                data = PacketFactory.Write(packet);
            }
            catch (Exception ex)
            {
                _logger.Exception(this, ex);
                return;
            }

          //  _logger.LogOutgoingPacket(this, packet);
            Socket.Send(data);
        }
    }
}
