using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ddon.Server.Network
{
    public class Client
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(Client));

        protected readonly ITcpSocket Socket;
        private readonly PacketFactory _packetFactory;
        private Challenge _challenge;

        public Client(ITcpSocket socket, PacketFactory packetFactory)
        {
            Socket = socket;
            _packetFactory = packetFactory;
            _challenge = null;
            Identity = socket.Identity;
        }

        public string Identity { get; protected set; }

        public DateTime PingTime { get; set; }

        public void Close()
        {
            Socket.Close();
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

        /// <summary>
        /// Send a Structure
        /// </summary>
        public void Send<TResStruct>(TResStruct res) where TResStruct : class, IPacketStructure, new()
        {
            StructurePacket<TResStruct> packet = new StructurePacket<TResStruct>(res);

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

            SendRaw(data);
            Logger.LogPacket(this, packet);
        }

        /// <summary>
        /// Sends raw bytes to the client, without any further processing
        /// </summary>
        public void SendRaw(byte[] data)
        {
            Socket.Send(data);
        }

        public void InitializeChallenge()
        {
            _challenge = new Challenge();
            byte[] challenge = _challenge.CreateClientCertChallenge();
            byte[] data;
            try
            {
                data = _packetFactory.WriteDataWithLengthPrefix(challenge);
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
