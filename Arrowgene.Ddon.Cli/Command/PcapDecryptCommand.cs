using System;
using System.Collections.Generic;
using System.Text;
using Arrowgene.Buffers;
using Arrowgene.Ddon.LoginServer.Dump;
using Arrowgene.Ddon.PacketLibrary;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Crypto;

namespace Arrowgene.Ddon.Cli.Command
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
            //   File.WriteAllBytes("F://char.bin", LoginDump.data_Dump_24);


            string pcapPath = parameter.Arguments[0];
            string key = parameter.Arguments[1];
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);


            PlFactory plFactory = new PlFactory();
            List<PlSession> sessions = plFactory.Create(pcapPath);
            List<PlPacket> encrypted = sessions[2].GetPackets();

          //  SplitPacketTest(encrypted, keyBytes);
            PrintPackets(encrypted, keyBytes);

            return CommandResultType.Continue;
        }

        private void StreamPacketTest(List<PlPacket> encrypted, byte[] keyBytes)
        {
            PacketFactory pf = new PacketFactory(new ServerSetting(), PacketIdResolver.GamePacketIdResolver);
            pf.SetCamelliaKey(keyBytes);

            // parse ddon packets
            int i = 0;
            PlPacket plpacket;
            List<Packet> packets = new List<Packet>();
            try
            {
                for (i = 0; i < encrypted.Count; i++)
                {
                    plpacket = encrypted[i];
                    List<Packet> readpackets = pf.Read(plpacket.Data);
                    packets.AddRange(readpackets);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("At:" + i + " Ex:" + ex);
            }

            foreach (Packet packet in packets)
            {
                Console.WriteLine(
                    $"Id:{packet.Id.GroupId}.{packet.Id.HandlerId}.{packet.Id.HandlerSubId}{Environment.NewLine}" +
                    $"Name:{packet.Id.Name}{Environment.NewLine}" +
                    $"{Util.HexDump(packet.Data)}"
                );
            }
        }

        private void PrintPackets(List<PlPacket> encrypted, byte[] keyBytes)
        {
            PacketFactory pf = new PacketFactory(new ServerSetting(), PacketIdResolver.GamePacketIdResolver);
            pf.SetCamelliaKey(keyBytes);
            List<Packet> packets = new List<Packet>();
            foreach (var encPacket in encrypted)
            {
                List<Packet> decPacket = pf.Read(encPacket.Data);
                packets.AddRange(decPacket);
            }
            string dump = PacketDump.DumpCSharpStruc(packets, "InGameDump");
            foreach (Packet packet in packets)
            {
                Console.WriteLine(
                    $"Id:{packet.Id.GroupId}.{packet.Id.HandlerId}.{packet.Id.HandlerSubId}{Environment.NewLine}" +
                    $"Name:{packet.Id.Name}{Environment.NewLine}" +
                    $"{Util.HexDump(packet.Data)}"
                );
            }
               
        }

        private void SplitPacketTest(List<PlPacket> encrypted, byte[] keyBytes)
        {
            PacketFactory pf = new PacketFactory(new ServerSetting(), PacketIdResolver.GamePacketIdResolver);
            pf.SetCamelliaKey(keyBytes);

            IBuffer tBuf = new StreamBuffer();
            foreach (PlPacket pl in encrypted)
            {
                tBuf.WriteBytes(pl.Data);
            }

            List<byte[]> splitData = new List<byte[]>();
            tBuf.SetPositionStart();
            while (tBuf.Position < tBuf.Size)
            {
                ushort len = tBuf.ReadUInt16(Endianness.Big);
                tBuf.Position -= 2;
                byte[] pdata = tBuf.ReadBytes(len + 2);
                splitData.Add(pdata);
            }

            Camellia c = new Camellia();
            //     Console.WriteLine(Util.HexDump(splitData[17]));

            //    File.WriteAllBytes("F://test.bin",splitData[17]);
            
            
            c.Decrypt(splitData[17].AsSpan(2), out Span<byte> output, keyBytes, CamelliaIv);
            Console.WriteLine(Util.HexDump(output.ToArray()));
            List<Packet> splitPacketsTest = pf.Read(splitData[17]);
            
            bool end = true;
        }


        public void Shutdown()
        {
        }
    }
}
