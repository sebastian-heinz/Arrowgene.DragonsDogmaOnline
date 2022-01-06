using System.Collections.Generic;
using Arrowgene.Ddo.PacketLibrary.KaitaiModel;

namespace Arrowgene.Ddo.PacketLibrary
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

            packets.Sort((a, b) => a.TsSec.CompareTo(b.TsSec));
            uint initialTsSec = packets[0].TsSec;
            uint initialTsUsec = packets[0].TsUsec;
            PlSession current = new PlSession();
            for (int packetNum = 0; packetNum < packets.Count; packetNum++)
            {
                Pcap.Packet pcapPacket = packets[packetNum];

                TcpSegment tcpSegment = GetTcpSegment(pcapPacket);
                if (tcpSegment == null)
                {
                    continue;
                }

                TcpFlag flag = TcpFlags.ParseFlags(tcpSegment.B12, tcpSegment.B13);
                if ((flag & TcpFlag.syn) != 0)
                {
                    // no data
                    continue;
                }

                // unknown status
                bool isLogin;
                ushort clientPort;
                if (tcpSegment.SrcPort == 52100)
                {
                    isLogin = true;
                    clientPort = tcpSegment.DstPort;
                }
                else if (tcpSegment.DstPort == 52100)
                {
                    isLogin = true;
                    clientPort = tcpSegment.SrcPort;
                }
                else if (tcpSegment.SrcPort == 52000)
                {
                    isLogin = false;
                    clientPort = tcpSegment.DstPort;
                }
                else if (tcpSegment.DstPort == 52000)
                {
                    isLogin = false;
                    clientPort = tcpSegment.SrcPort;
                }
                else
                {
                    // not ddo port
                    continue;
                }

                if (current.IsLogin == null)
                {
                    // new session
                    current.IsLogin = isLogin;
                    current.ClientPort = clientPort;
                }
                else if (current.IsLogin != isLogin || current.ClientPort != clientPort)
                {
                    // state change
                    sessions.Add(current);
                    current = new PlSession();
                    current.IsLogin = isLogin;
                    current.ClientPort = clientPort;
                }

                if (!(pcapPacket.Body is EthernetFrame ethFrame))
                {
                    return null;
                }

                PlPacket plPacket = new PlPacket();
                plPacket.Data = tcpSegment.Body;
                plPacket.SrcPort = tcpSegment.SrcPort;
                plPacket.DstPort = tcpSegment.DstPort;
                plPacket.Flag = flag;
                plPacket.TsSec = pcapPacket.TsSec;
                plPacket.TsUsec = pcapPacket.TsUsec;
                plPacket.InitialTsSec = initialTsSec;
                plPacket.InitialTsUsec = initialTsUsec;
                plPacket.PacketNum = (uint)packetNum + 1;
                current.Add(plPacket);
            }

            return sessions;
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

            if (tcpSegment.Body == null || tcpSegment.Body.Length <= 0)
            {
                // no data
                return null;
            }

            return tcpSegment;
        }
    }
}
