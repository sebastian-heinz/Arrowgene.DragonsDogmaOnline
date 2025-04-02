using System;
using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.Ddon.Server.Network
{
    public class Client
    {
        private readonly ServerLogger Logger;

        protected readonly ITcpSocket Socket;
        private readonly PacketFactory _packetFactory;
        private Challenge _challenge;

        /**
         * Roundtrip Challenge completed
         * The client considered to be initialized with crypto key
         * and able to parse packet headers.
         */
        private bool _challengeCompleted;

        public Client(ITcpSocket socket, PacketFactory packetFactory)
        {
            Logger = LogProvider.Logger<ServerLogger>(GetType());
            Socket = socket;
            _packetFactory = packetFactory;
            _challenge = null;
            Identity = socket.Identity;
            _challengeCompleted = false;
        }

        public string Identity { get; protected set; }

        public DateTime PingTime { get; set; }

        public void SetChallengeCompleted(bool challengeCompleted)
        {
            _challengeCompleted = challengeCompleted;
            Logger.Info(this, $"SetChallengeCompleted:{_challengeCompleted}");
        }

        public void Close()
        {
            Socket.Close();
        }

        public List<IPacket> Receive(byte[] data)
        {
            List<IPacket> packets;
            try
            {
                packets = _packetFactory.Read(data);
            }
            catch (Exception ex)
            {
                Logger.Exception(this, ex);
                packets = new List<IPacket>();
            }

            foreach (IPacket packet in packets)
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

        public void Enqueue<TResStruct>(TResStruct res, PacketQueue queue)
            where TResStruct : class, IPacketStructure, new()
        {
            StructurePacket<TResStruct> packet = new StructurePacket<TResStruct>(res);
            queue.Enqueue((this, packet));
        }

        public void Enqueue(Packet res, PacketQueue queue)
        {
            queue.Enqueue((this, res));
        }

        public void Send(Packet packet)
        {
            if (!_challengeCompleted 
                && packet.Id != PacketId.S2C_CERT_CLIENT_CHALLENGE_RES
                && packet.Id != PacketId.L2C_CLIENT_CHALLENGE_RES
                )
            {
                // at this point in time we only allow to send S2C_CERT_CLIENT_CHALLENGE_RES
                // only after receiving the first client packet, we can assume the client is able
                // to parse packets headers and process other packets.
                Logger.Debug(this,
                    $"Tried to send Packet:\"{packet.PrintHeader()}\", while client not yet considered ready");
                return;
            }

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


        public Challenge.Response HandleChallenge(C2SCertClientChallengeReq request)
        {
            Challenge.Response challenge = _challenge.HandleClientCertChallenge(request);
            _challenge = null;
            if (challenge.Error)
            {
                Logger.Error(this, "Failed CertChallenge");
            }

            _packetFactory.SetCamelliaKey(challenge.CamelliaKey);
            return challenge;
        }

        public Challenge.Response HandleChallenge(C2LClientChallengeReq request)
        {
            Challenge.Response challenge = _challenge.HandleClientCertChallenge(request);
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
