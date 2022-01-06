using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Arrowgene.Buffers;
using Arrowgene.Ddo.GameServer;
using Arrowgene.Ddo.GameServer.Network;
using Arrowgene.Ddo.PacketLibrary.KaitaiModel;
using Arrowgene.Ddo.Shared;
using Arrowgene.Ddo.Shared.Crypto;

namespace Arrowgene.Ddo.Cli.Command
{
    public class PcapDecryptCommand : ICommand
    {
        public string Key => "pcap";

        public string Description =>
            $"Decrypt Pcaps Data";

        private static readonly byte[] CamelliaIv = new byte[]
        {
            0x24, 0x63, 0x62, 0x4D, 0x36, 0x57, 0x50, 0x29, 0x61, 0x58, 0x3D, 0x25, 0x4A, 0x5E, 0x7A, 0x41
        };

        public CommandResultType Run(CommandParameter parameter)
        {
            string pcapPath = parameter.Arguments[0];
            string key = parameter.Arguments[1];
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            // split up sessions
            Pcap pcap = Pcap.FromFile(pcapPath);

            int current = 0;
            foreach (Pcap.Packet pcapPacket in pcap.Packets)
            {
                current++;
                TcpSegment tcpSegment = GetDdoPacketTcpSegment(pcapPacket);
                if (tcpSegment == null)
                {
                    continue;
                }
                
                byte[] payload = tcpSegment.Body;
                ushort srcPort = tcpSegment.SrcPort;
                ushort dstPort = tcpSegment.DstPort;
            }


            // PacketFactory pf = new PacketFactory(new GameServerSetting());
            // pf.SetCamelliaKey(keyBytes);
            // List<Packet> packets = pf.Read(data);
            // foreach (Packet packet in packets)
            // {
            //     Console.WriteLine(
            //         $"Id:{packet.Id.GroupId}.{packet.Id.HandlerId}.{packet.Id.HandlerSubId}{Environment.NewLine}" +
            //         $"Name:{packet.Id.Name}{Environment.NewLine}" +
            //         $"{Util.HexDump(packet.Data)}"
            //     );
            // }
            return CommandResultType.Continue;
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

            // BitArray b12 = new BitArray(new byte[]{tcpSegment.B12});
            // Reverse(b12); // byte order
            /*
             * Data offset (4 bits)
             * Reserved (3 bits) For future use and should be set to zero.
             * NS (1 bit): ECN-nonce - concealment protection
             */ 
            
            BitArray b13 = new BitArray(new byte[]{tcpSegment.B13});
            Reverse(b13); // byte order
            /*
            * CWR (1 bit): Congestion window reduced
            * ECE (1 bit): ECN-Echo has a dual role, depending on the value of the SYN flag.
            * URG (1 bit): Indicates that the Urgent pointer field is significant
            * ACK (1 bit): Indicates that the Acknowledgment field is significant.
            * PSH (1 bit): Push function. Asks to push the buffered data to the receiving application.
            * RST (1 bit): Reset the connection
            * SYN (1 bit): Synchronize sequence numbers. Only the first packet sent from each end should have this flag set. Some other flags and fields change meaning based on this flag, and some are only valid when it is set, and others when it is clear.
            * FIN (1 bit): Last packet from sender
            */
            bool ack = b13.Get(3);
            bool psh = b13.Get(4);
            bool rst = b13.Get(5);
            bool syn = b13.Get(6);

            if (syn)
            {
                // no data
                return null;
            }
            if (rst)
            {
                // no data
                return null;
            }




            return tcpSegment;
        }

        private void Reverse(BitArray array)
        {
            int length = array.Length;
            int mid = (length / 2);

            for (int i = 0; i < mid; i++)
            {
                bool bit = array[i];
                array[i] = array[length - i - 1];
                array[length - i - 1] = bit;
            }    
        }
        
        private TcpSegment GetDdoPacketTcpSegment(Pcap.Packet pcapPacket)
        {
            TcpSegment tcpSegment = GetTcpSegment(pcapPacket);

            if (tcpSegment == null)
            {
                return null;
            }
            
            if (
                tcpSegment.SrcPort != 52100 /* login */
                && tcpSegment.DstPort != 52100 /* login */
                && tcpSegment.SrcPort != 52000 /* game */
                && tcpSegment.DstPort != 52000 /* game */
                )
            {
                return null;
            }

            return tcpSegment;
        }

        public void Shutdown()
        {
        }
    }
}
