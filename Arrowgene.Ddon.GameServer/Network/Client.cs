using System;
using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Logging;
using Arrowgene.Ddon.GameServer.Model;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ddon.GameServer.Network
{
    public class Client
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(Client));

        private ITcpSocket _socket;
        private PacketFactory _packetFactory;
        private Challenge _challenge;

        public Client(ITcpSocket socket, PacketFactory packetFactory)
        {
            _socket = socket;
            _packetFactory = packetFactory;
            State = new ClientState();
            _challenge = null;
        }

        public string Identity { get; private set; }
        public ClientState State { get; private set; }
    

        public void Close()
        {
            _socket.Close();
        }

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

            foreach (Packet packet in packets)
            {
                Logger.LogPacket(this, packet);
            }

            return packets;
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

            SendRaw(data);
            Logger.LogPacket(this, packet);
        }

        /// <summary>
        /// Sends raw bytes to the client, without any further processing
        /// </summary>
        public void SendRaw(byte[] data)
        {
            Logger.Send(_socket, data);
            _socket.Send(data);
        }

        public void InitializeChallenge()
        {
            _challenge = new Challenge();
            byte[] challenge = _challenge.CreateClientCertChallenge();
            Packet packet = new Packet(challenge);

            byte[] data;
            try
            {
                data = _packetFactory.WriteWithoutHeader(packet);
            }
            catch (Exception ex)
            {
                Logger.Exception(this, ex);
                return;
            }

            SendRaw(data);
        }

        public Challenge.Response HandleChallenge(byte[] data)
        {
            Challenge.Response challenge = _challenge.HandleClientCertChallenge(data);
            _challenge = null;
            if (challenge.Error)
            {
                Logger.Error(this, "Failed CertChallenge");
            }

            _packetFactory.SetCamelliaKey(challenge.CamelliaKey);
            return challenge;
        }
    }
}
