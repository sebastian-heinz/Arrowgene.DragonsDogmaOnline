using System.Collections.Generic;
using Arrowgene.Ddon.PacketLibrary.KaitaiModel;

namespace Arrowgene.Ddon.PacketLibrary
{
    public class PlFactory
    {
        public PlFactory()
        {
        }

        public List<PlSession> Create(string pcapPath)
        {
            List<PlSession> sessions = new List<PlSession>();
            Pcap pcap = Pcap.FromFile(pcapPath);
            List<Pcap.Packet> packets = pcap.Packets;
            if (packets.Count <= 0)
            {
                return sessions;
            }

            uint initialTsSec = packets[0].TsSec;
            uint initialTsUsec = packets[0].TsUsec;

            List<List<Pcap.Packet>> streams = ExtractStreams(packets);
            foreach (List<Pcap.Packet> stream in streams)
            {
                List<PlFactoryPacket> plFactoryPackets = ExtractPackets(stream, initialTsSec, initialTsUsec);
                PlSession session = new PlSession();
                foreach (PlFactoryPacket plFactoryPacket in plFactoryPackets)
                {
                    PlPacket plPacket = new PlPacket();
                    plPacket.Data = plFactoryPacket.Data;
                    plPacket.Timestamp = plFactoryPacket.Timestamp;
                    plPacket.Direction = plFactoryPacket.GetDirection();
                    session.Add(plPacket);
                }

                sessions.Add(session);
            }

            return sessions;
        }

        private int GetNext(int packetNum, out TcpSegment tcpSegment, List<Pcap.Packet> packets)
        {
            for (int i = packetNum; i < packets.Count; i++)
            {
                Pcap.Packet pcapPacket = packets[i];
                tcpSegment = GetTcpSegment(pcapPacket);
                if (tcpSegment == null)
                {
                    continue;
                }

                if (!(
                    tcpSegment.SrcPort == 52100
                    || tcpSegment.DstPort == 52100
                    || tcpSegment.SrcPort == 52000
                    || tcpSegment.DstPort == 52000)
                )
                {
                    // not ddon packet
                    continue;
                }

                return i;
            }

            tcpSegment = null;
            return -1;
        }

        private List<PlFactoryPacket> ExtractPackets(List<Pcap.Packet> packets, uint initialTsSec,
            uint initialTsUsec)
        {
            List<PlFactoryPacket> plFactoryPackets = new List<PlFactoryPacket>();
            for (int packetNum = 0; packetNum < packets.Count; packetNum++)
            {
                Pcap.Packet pcapPacket = packets[packetNum];
                TcpSegment tcpSegment = GetTcpSegment(pcapPacket);
                if (tcpSegment == null)
                {
                    continue;
                }

                if (tcpSegment.Body == null || tcpSegment.Body.Length <= 0)
                {
                    // no data
                    continue;
                }

                TcpFlag flag = TcpFlags.ParseFlags(tcpSegment.B12, tcpSegment.B13);
                if ((flag & TcpFlag.fin) != 0)
                {
                    // no data
                    continue;
                }

                if ((flag & TcpFlag.syn) != 0)
                {
                    // no data
                    continue;
                }

                PlFactoryPacket plFactoryPacket = new PlFactoryPacket();
                plFactoryPacket.Data = tcpSegment.Body;
                plFactoryPacket.SrcPort = tcpSegment.SrcPort;
                plFactoryPacket.DstPort = tcpSegment.DstPort;
                plFactoryPacket.Flag = flag;
                plFactoryPacket.TsSec = pcapPacket.TsSec;
                plFactoryPacket.TsUsec = pcapPacket.TsUsec;
                plFactoryPacket.InitialTsSec = initialTsSec;
                plFactoryPacket.InitialTsUsec = initialTsUsec;
                plFactoryPacket.PacketNum = pcapPacket.PacketNum;
                plFactoryPackets.Add(plFactoryPacket);
            }


            return plFactoryPackets;
        }

        private List<List<Pcap.Packet>> ExtractStreams(List<Pcap.Packet> pcapSession)
        {
            List<List<Pcap.Packet>> streams = new List<List<Pcap.Packet>>();
            int packetNum = 0;
            bool hasSegments = true;
            while (hasSegments)
            {
                packetNum = GetNext(packetNum, out TcpSegment tcpSegment, pcapSession);
                if (tcpSegment == null)
                {
                    hasSegments = false;
                    continue;
                }

                TcpFlag flag = TcpFlags.ParseFlags(tcpSegment.B12, tcpSegment.B13);
                if (!flag.HasFlag(TcpFlag.syn))
                {
                    // not beginning
                    packetNum++;
                    continue;
                }

                if (flag.HasFlag(TcpFlag.ack))
                {
                    // not beginning
                    packetNum++;
                    continue;
                }

                List<Pcap.Packet> stream = IsolateStream(tcpSegment, packetNum, pcapSession);
                streams.Add(stream);
                packetNum++;
            }

            return streams;
        }

        private List<Pcap.Packet> IsolateStream(TcpSegment synSegment, int startPacketNum, List<Pcap.Packet> pcapSession)
        {
            uint aSeq = synSegment.SeqNum;
            uint aSegmentLength = 0;
            uint aAckNum = synSegment.AckNum;
            uint bSeq = 0;
            uint bSegmentLength = 0;
            uint bAckNum = 0;
            bool accepted = false;
            List<Pcap.Packet> stream = new List<Pcap.Packet>();
            for (int packetNum = startPacketNum; packetNum < pcapSession.Count; packetNum++)
            {
                Pcap.Packet pcapPacket = pcapSession[packetNum];
                TcpSegment tcpSegment = GetTcpSegment(pcapPacket);
                if (tcpSegment == null)
                {
                    continue;
                }

                bool direction;
                if (synSegment.SrcPort == tcpSegment.SrcPort && synSegment.DstPort == tcpSegment.DstPort)
                {
                    // A -> B
                    direction = true;
                }
                else if (synSegment.SrcPort == tcpSegment.DstPort && synSegment.DstPort == tcpSegment.SrcPort)
                {
                    // B -> A
                    direction = false;
                }
                else
                {
                    // not related to stream
                    continue;
                }

                TcpFlag flag = TcpFlags.ParseFlags(tcpSegment.B12, tcpSegment.B13);
                if ((flag & TcpFlag.fin) != 0)
                {
                    // end of stream
                    break;
                }

                if (flag.HasFlag(TcpFlag.syn) && flag.HasFlag(TcpFlag.ack))
                {
                    // connection accepted
                    bSeq = tcpSegment.SeqNum;
                    bSegmentLength = 0;
                    bAckNum = tcpSegment.AckNum;
                    accepted = true;
                    continue;
                }

                if (!accepted)
                {
                    continue;
                }

                if (direction)
                {
                    if (tcpSegment.SeqNum < aSeq)
                    {
                        // retransmission
                        for (int r = stream.Count - 1; r > 0; r--)
                        {
                            Pcap.Packet errPacket = stream[r];
                            TcpSegment errSegment = GetTcpSegment(errPacket);
                            if (errSegment.SeqNum == tcpSegment.SeqNum)
                            {
                                stream[r] = pcapPacket;
                            }
                        }

                        continue;
                    }

                    aSeq = tcpSegment.SeqNum;
                    aSegmentLength = (uint) tcpSegment.Body.Length;
                    aAckNum = tcpSegment.AckNum;
                }
                else
                {
                    if (tcpSegment.SeqNum < bSeq)
                    {
                        // retransmission
                        for (int r = stream.Count - 1; r > 0; r--)
                        {
                            Pcap.Packet errPacket = stream[r];
                            TcpSegment errSegment = GetTcpSegment(errPacket);
                            if (errSegment.SeqNum == tcpSegment.SeqNum)
                            {
                                stream[r] = pcapPacket;
                            }
                        }

                        continue;
                    }

                    bSeq = tcpSegment.SeqNum;
                    bSegmentLength = (uint) tcpSegment.Body.Length;
                    bAckNum = tcpSegment.AckNum;
                }

                stream.Add(pcapPacket);
            }

            return stream;
        }

        private TcpSegment GetTcpSegment(Pcap.Packet pcapPacket)
        {
            if (!(pcapPacket.Body is EthernetFrame ethFrame))
            {
                return null;
            }

            if (!(ethFrame.Body is Ipv4Packet ipv4Packet))
            {
                return null;
            }

            ProtocolBody protocolBody = ipv4Packet.Body;
            if (protocolBody == null)
            {
                return null;
            }

            if (protocolBody.Protocol != ProtocolBody.ProtocolEnum.Tcp)
            {
                return null;
            }

            if (!(protocolBody.Body is TcpSegment tcpSegment))
            {
                return null;
            }

            return tcpSegment;
        }
    }
}
