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

            //   packets.Sort((a, b) => a.TsSec.CompareTo(b.TsSec));
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
            for (int packetNum = 0; packetNum < pcapSession.Count; packetNum++)
            {
                Pcap.Packet pcapPacket = pcapSession[packetNum];
                TcpSegment tcpSegment = GetTcpSegment(pcapPacket);
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

                TcpFlag flag = TcpFlags.ParseFlags(tcpSegment.B12, tcpSegment.B13);
                if ((flag & TcpFlag.syn) != 0 && (flag & TcpFlag.ack) != 0)
                {
                    // connection accepted
                    List<Pcap.Packet> stream = IsolateStream(tcpSegment, packetNum, pcapSession);
                    streams.Add(stream);
                }
            }

            return streams;
        }

        private List<Pcap.Packet> IsolateStream(TcpSegment startSegment, int startPacketNum,
            List<Pcap.Packet> pcapSession)
        {
            List<Pcap.Packet> stream = new List<Pcap.Packet>();
            for (int packetNum = startPacketNum; packetNum < pcapSession.Count; packetNum++)
            {
                Pcap.Packet pcapPacket = pcapSession[packetNum];
                TcpSegment tcpSegment = GetTcpSegment(pcapPacket);
                if (tcpSegment == null)
                {
                    continue;
                }

                // match
                if (!(
                    (startSegment.SrcPort == tcpSegment.SrcPort && startSegment.DstPort == tcpSegment.DstPort)
                    || (startSegment.DstPort == tcpSegment.SrcPort && startSegment.SrcPort == tcpSegment.DstPort)
                ))
                {
                    continue;
                }

                TcpFlag flag = TcpFlags.ParseFlags(tcpSegment.B12, tcpSegment.B13);
                if ((flag & TcpFlag.fin) != 0)
                {
                    // end of stream
                    break;
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
