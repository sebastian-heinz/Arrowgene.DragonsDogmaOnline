using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Arrowgene.Ddon.PacketLibrary;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared;

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

        /// <summary>
        /// json|pcap
        /// json/pcap path
        /// decryption key
        /// ex.: pcap F:\\capture.pcap 00112233445566778899AABBCCDDEEFF
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public CommandResultType Run(CommandParameter parameter)
        {
            List<Packet> packets = new List<Packet>();
            if (parameter.Arguments[0] == "json")
            {
                String json = File.ReadAllText(parameter.Arguments[1]);
                PlPacketStream packetStream = JsonSerializer.Deserialize<PlPacketStream>(json);
                if (packetStream.Encrypted)
                {
                    byte[] keyBytes = Encoding.UTF8.GetBytes(parameter.Arguments[2]);
                    List<Packet> decrypted = Decrypt(packetStream.Packets, keyBytes);
                    packets.AddRange(decrypted);
                }
                else
                {
                    List<Packet> converted = Convert(packetStream.Packets);
                    packets.AddRange(converted);
                }
            }
            else if (parameter.Arguments[0] == "pcap")
            {
                PlFactory plFactory = new PlFactory();
                List<PlSession> sessions = plFactory.Create(parameter.Arguments[1]);
                List<PlPacket> encrypted = sessions[2].GetPackets(); // hardcoded session index
                byte[] keyBytes = Encoding.UTF8.GetBytes(parameter.Arguments[2]);
                List<Packet> decrypted = Decrypt(encrypted, keyBytes);
                packets.AddRange(decrypted);
            }
            else
            {
                return CommandResultType.Continue;
            }

            PrintPackets(packets);
            return CommandResultType.Continue;
        }

        private void PrintPackets(List<Packet> packets)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Packet packet in packets)
            {
                string text = $"Id:{packet.Id.GroupId}.{packet.Id.HandlerId}.{packet.Id.HandlerSubId}{Environment.NewLine}" +
                              $"Name:{packet.Id.Name}{Environment.NewLine}" +
                              $"{Util.HexDump(packet.Data)}";
                Console.WriteLine(text);
                sb.Append(text);
            }

            //   string dump = PacketDump.DumpCSharpStruc(packets, "GameFull");
            //     File.WriteAllText("F://game_full.txt", sb.ToString());
        }

        private List<Packet> Convert(List<PlPacket> plPackets)
        {
            List<Packet> packets = new List<Packet>();
            foreach (PlPacket plPacket in plPackets)
            {
                // parsng decrypted packets not supported in packet factory
            }

            return packets;
        }

        private List<Packet> Decrypt(List<PlPacket> encrypted, byte[] keyBytes)
        {
            PacketFactory pfServer = new PacketFactory(new ServerSetting(), PacketIdResolver.GamePacketIdResolver);
            PacketFactory pfClient = new PacketFactory(new ServerSetting(), PacketIdResolver.GamePacketIdResolver);
            pfServer.SetCamelliaKey(keyBytes);
            pfClient.SetCamelliaKey(keyBytes);
            List<Packet> packets = new List<Packet>();

            foreach (PlPacket encPacket in encrypted)
            {
                try
                {
                    if (encPacket.Direction == "S2C")
                    {
                        List<Packet> decrypted = pfClient.Read(encPacket.Data);
                        packets.AddRange(decrypted);
                    }
                    else if (encPacket.Direction == "C2S")
                    {
                        List<Packet> decrypted = pfServer.Read(encPacket.Data);
                        packets.AddRange(decrypted);
                    }
                }
                catch (Exception ex)
                {
                    // try whatever we can
                }
            }

            return packets;
        }

        public void Shutdown()
        {
        }
    }
}
